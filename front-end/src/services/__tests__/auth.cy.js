import { login, register, logout, isAuthenticated, getToken } from '../auth'

describe('Auth Service', () => {
  beforeEach(() => {
    // Limpa o localStorage antes de cada teste
    localStorage.clear()
  })
  beforeEach(() => {
    // Limpa o localStorage antes de cada teste
    localStorage.clear()
    // Limpa todos os mocks de requisições
    cy.intercept('**/api/Auth/**', (req) => {
      req.reply({
        statusCode: 404,
        body: 'No route matched'
      })
    })
  })

  it('should login successfully', () => {
    const mockToken = 'fake-jwt-token'
    
    cy.intercept('POST', '**/api/Auth/login', {
      statusCode: 200,
      body: { token: mockToken }
    }).as('loginRequest')

    cy.wrap(login('test@email.com', 'password')).then((token) => {
      expect(token).to.equal(mockToken)
      expect(localStorage.getItem('token')).to.equal(mockToken)
      cy.get('@loginRequest').its('request.body').should('deep.equal', {
        email: 'test@email.com',
        senha: 'password'
      })
    })
  })

  it('should handle login error', () => {
    const mockResponse = { message: 'Invalid credentials' }
    
    cy.intercept('POST', 'http://localhost:5139/api/Auth/login', (req) => {
      expect(req.body).to.deep.equal({
        email: 'test@example.com',
        senha: 'password123'
      })
      req.reply({
        statusCode: 200,
        body: mockResponse
      })
    }).as('loginRequest')

    cy.wrap(login('test@email.com', 'wrong-password'))
      .should('be.rejected')
      .then((error) => {
        expect(error.message).to.equal(mockResponse.message)
        expect(localStorage.getItem('token')).to.be.null
      })
  })

  it('should register successfully', () => {
    const mockToken = 'new-user-token'
    
    cy.intercept('POST', '**/api/Auth/register', {
      statusCode: 200,
      body: { token: mockToken }
    }).as('registerRequest')

    cy.wrap(register('new@email.com', 'password123', 'New User')).then((token) => {
      expect(token).to.equal(mockToken)
      expect(localStorage.getItem('token')).to.equal(mockToken)
      cy.get('@registerRequest').its('request.body').should('deep.equal', {
        email: 'new@email.com',
        senha: 'password123',
        nomeUsuario: 'New User'
      })
    })
  })

  it('should handle register error', () => {
    const mockResponse = { message: 'Email already exists' }
    
    cy.intercept('POST', 'http://localhost:5139/api/Auth/register', (req) => {
      expect(req.body).to.deep.equal({
        email: 'test@example.com',
        senha: 'password123',
        nomeUsuario: 'Test User'
      })
      req.reply({
        statusCode: 200,
        body: mockResponse
      })
    }).as('registerRequest')

    cy.wrap(register('existing@email.com', 'password', 'Existing User'))
      .should('be.rejected')
      .then((error) => {
        expect(error.message).to.equal(mockResponse.message)
        expect(localStorage.getItem('token')).to.be.null
      })
  })

  it('should logout and clear token', () => {
    localStorage.setItem('token', 'existing-token')
    logout()
    expect(localStorage.getItem('token')).to.be.null
  })

  it('should check authentication status', () => {
    expect(isAuthenticated()).to.be.false
    localStorage.setItem('token', 'some-token')
    expect(isAuthenticated()).to.be.true
  })

  it('should get token', () => {
    const mockToken = 'stored-token'
    localStorage.setItem('token', mockToken)
    expect(getToken()).to.equal(mockToken)
  })
})
