describe('Lista de Compras E2E', () => {
  beforeEach(() => {
    // Visita a página inicial
    cy.visit('http://localhost:5173')
    // Limpa o localStorage antes de cada teste
    cy.clearLocalStorage()
  })
  const user = {
    email: 'test@test.com',
    password: 'password1234',
    name: 'User Test'
  }



  it('should complete full user journey', () => {
    // 1. Registro de usuário
    cy.get('[data-cy=register-link]').click()
    cy.url().should('include', '/cadastrar')

    cy.get('[data-cy=register-email]').type(user.email)
    cy.get('[data-cy=register-name]').type(user.name)
    cy.get('[data-cy=register-password]').type(user.password)
    cy.get('[data-cy=register-confirm-password]').type(user.password)
    cy.get('[data-cy=register-submit]').click()

    // Espera o registro ser concluído e verifica se foi redirecionado para a página principal
    cy.url().should('include', '/home', { timeout: 10000 })

    // 2. Criar uma nova lista
    cy.get('[data-cy=new-list-button]').click()
    cy.url().should('include', '/lista')

    // Adiciona nome da lista
    cy.get('[data-cy=list-name-input]').clear().type('Supermercado')

    // 3. Adicionar itens à lista
    cy.get('[data-cy=item-name-input]').type('Arroz')
    cy.get('[data-cy=item-quantity-input]').type('2')
    cy.get('[data-cy=new-item-button]').click()

    // Verifica se o item foi adicionado
    cy.contains('2 - Arroz').should('be.visible')

    // 4. Salvar a lista
    cy.get('[data-cy=save-list-button]').click()

    // Voltar para a home
    cy.url().should('include', '/home')

    // 5. Verificar se a lista foi criada
    cy.get('[data-cy=list-item]').should('contain', 'Supermercado')

    // 6. Excluir a lista
    cy.get('[data-cy=delete-list-button]').click()

    // Verifica se a lista foi removida
    cy.contains('Supermercado').should('not.exist')

    // 7. Fazer logout
    cy.get('[data-cy=logout-button]').click()
    cy.clearLocalStorage()
    cy.url().should('include', '/')

    // 8. Login novamente
    cy.get('[data-cy=login-link]').click()
    cy.url().should('include', '/login')
    
    cy.get('[data-cy=login-email]').type(user.email)
    cy.get('[data-cy=login-password]').type(user.password)
    cy.get('[data-cy=login-submit]').click()

    // Verifica se foi redirecionado para a home
    cy.url().should('include', '/home', { timeout: 10000 })
  })

  it('should handle failed login', () => {
    cy.visit('/login')
    cy.get('[data-cy=login-email]').type('wrong@email.com')
    cy.get('[data-cy=login-password]').type('wrongpassword')
    cy.get('[data-cy=login-submit]').click()

    // Verifica se continua na página de login
    cy.url().should('include', '/login')
  })

  it('should handle network errors', () => {
    // Simula erro de rede no login
    cy.intercept('POST', '**/api/Auth/login', {
      forceNetworkError: true
    })

    cy.visit('/login')
    cy.get('[data-cy=login-email]').type(user.email)
    cy.get('[data-cy=login-password]').type(user.password)
    cy.get('[data-cy=login-submit]').click()

    // Verifica se continua na página de login
    cy.url().should('include', '/login')
  })
})
