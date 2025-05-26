import { getListas, addLista, updateLista, deleteLista } from '../lista';

describe('Lista Service', () => {
  const API_URL = 'http://localhost:5139/api/Lista';
  const mockToken = 'fake-jwt-token';

  beforeEach(() => {
    // Seta o token no contexto da janela real
    cy.window().then((win) => {
      win.localStorage.clear();
      win.localStorage.setItem('token', mockToken);
    });

    // Desativa interceptador 404 genérico que atrapalha
    // cy.intercept('http://localhost:5139/api/Lista/**', (req) => {
    //   req.reply({
    //     statusCode: 404,
    //     body: 'No route matched'
    //   });
    // });
  });

  it('should get all lists', () => {
    const mockListas = [
      { id: 1, nomeLista: 'Lista 1' },
      { id: 2, nomeLista: 'Lista 2' }
    ];

    cy.intercept('GET', API_URL, (req) => {
      expect(req.headers).to.have.property('authorization', `Bearer ${mockToken}`);
      req.reply({
        statusCode: 200,
        body: mockListas
      });
    }).as('getListasRequest');

    cy.wrap(getListas()).then((listas) => {
      expect(listas).to.deep.equal(mockListas);
      cy.get('@getListasRequest').its('request.headers')
        .should('include', {
          authorization: `Bearer ${mockToken}`
        });
    });
  });

  it('should add a new list', () => {
    const mockNewLista = { id: 1, nomeLista: 'Nova Lista' };

    cy.intercept('POST', API_URL, (req) => {
      expect(req.headers).to.have.property('authorization', `Bearer ${mockToken}`);
      expect(req.body).to.deep.equal({
        nomeLista: 'Nova Lista'
      });
      req.reply({
        statusCode: 200,
        body: mockNewLista
      });
    }).as('addListaRequest');

    cy.wrap(addLista('Nova Lista')).then((lista) => {
      expect(lista).to.deep.equal(mockNewLista);
      cy.get('@addListaRequest').then((interception) => {
        expect(interception.request.headers).to.include({
          authorization: `Bearer ${mockToken}`
        });
        expect(interception.request.body).to.deep.equal({
          nomeLista: 'Nova Lista'
        });
      });
    });
  });

  it('should update a list', () => {
    const mockUpdatedLista = { id: 1, nomeLista: 'Lista Atualizada' };

    cy.intercept('PUT', `${API_URL}/1`, (req) => {
      expect(req.headers).to.have.property('authorization', `Bearer ${mockToken}`);
      expect(req.body).to.deep.equal({
        nomeLista: 'Lista Atualizada'
      });
      req.reply({
        statusCode: 200,
        body: mockUpdatedLista
      });
    }).as('updateListaRequest');

    cy.wrap(updateLista(1, 'Lista Atualizada')).then((lista) => {
      expect(lista).to.deep.equal(mockUpdatedLista);
      cy.get('@updateListaRequest').then((interception) => {
        expect(interception.request.headers).to.include({
          authorization: `Bearer ${mockToken}`
        });
        expect(interception.request.body).to.deep.equal({
          nomeLista: 'Lista Atualizada'
        });
      });
    });
  });

  it('should delete a list', () => {
    cy.intercept('DELETE', `${API_URL}/1`, (req) => {
      expect(req.headers).to.have.property('authorization', `Bearer ${mockToken}`);
      req.reply({
        statusCode: 200
      });
    }).as('deleteListaRequest');

    cy.wrap(deleteLista(1)).then(() => {
      cy.get('@deleteListaRequest').its('request.headers')
        .should('include', {
          authorization: `Bearer ${mockToken}`
        });
    });
  });

  it('should handle error when getting lists', () => {
    const errorMessage = 'Failed to fetch lists';

    cy.intercept('GET', '**/api/Lista', {
      statusCode: 400,
      body: errorMessage
    });

    cy.wrap(getListas())
      .should('be.rejected')
      .then((error) => {
        expect(error.message).to.equal(errorMessage);
      });
  });

  it('should handle error when adding list', () => {
    const errorMessage = 'Failed to add list';

    cy.intercept('POST', '**/api/Lista', {
      statusCode: 400,
      body: errorMessage
    });

    cy.wrap(addLista('Nova Lista'))
      .should('be.rejected')
      .then((error) => {
        expect(error.message).to.equal(errorMessage);
      });
  });
});
