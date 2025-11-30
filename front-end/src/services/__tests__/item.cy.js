import { getItens, addItem, updateItem, deleteItem } from '../item';

describe('Item Service', () => {
  const API_URL = 'http://localhost:5139/api/lista';
  const mockToken = 'fake-jwt-token';

  beforeEach(() => {
    // Garante que o token está no contexto da janela do navegador real
    cy.window().then((win) => {
      win.localStorage.clear();
      win.localStorage.setItem('token', mockToken);
    });
  });

  it('should get items from a list', () => {
    const mockItems = [
      { id: 1, nomeItem: 'Item 1', quantidade: 1 },
      { id: 2, nomeItem: 'Item 2', quantidade: 2 }
    ];

    cy.intercept('GET', `${API_URL}/1/item`, (req) => {
      expect(req.headers).to.have.property('authorization', `Bearer ${mockToken}`);
      req.reply({
        statusCode: 200,
        body: mockItems
      });
    }).as('getItensRequest');

    cy.wrap(getItens(1)).then((items) => {
      expect(items).to.deep.equal(mockItems);
      cy.get('@getItensRequest').its('request.headers')
        .should('include', {
          authorization: `Bearer ${mockToken}`
        });
    });
  });

  it('should add a new item', () => {
    const mockNewItem = { id: 1, nomeItem: 'Novo Item', quantidade: 1 };

    cy.intercept('POST', `${API_URL}/1/item`, (req) => {
      expect(req.headers).to.have.property('authorization', `Bearer ${mockToken}`);
      expect(req.body).to.deep.equal({
        nomeItem: 'Novo Item',
        quantidade: 1
      });
      req.reply({
        statusCode: 200,
        body: mockNewItem
      });
    }).as('addItemRequest');

    cy.wrap(addItem(1, 'Novo Item', 1)).then((item) => {
      expect(item).to.deep.equal(mockNewItem);
      cy.get('@addItemRequest').then((interception) => {
        expect(interception.request.headers).to.include({
          authorization: `Bearer ${mockToken}`
        });
        expect(interception.request.body).to.deep.equal({
          nomeItem: 'Novo Item',
          quantidade: 1
        });
      });
    });
  });

  it('should update an item', () => {
    const mockUpdatedItem = { id: 1, nomeItem: 'Item Atualizado', quantidade: 2 };

    cy.intercept('PUT', `${API_URL}/1/item/1`, (req) => {
      expect(req.headers).to.have.property('authorization', `Bearer ${mockToken}`);
      expect(req.body).to.deep.equal({
        nomeItem: 'Item Atualizado',
        quantidade: 2
      });
      req.reply({
        statusCode: 200,
        body: mockUpdatedItem
      });
    }).as('updateItemRequest');

    cy.wrap(updateItem(1, 1, 'Item Atualizado', 2)).then((item) => {
      expect(item).to.deep.equal(mockUpdatedItem);
      cy.get('@updateItemRequest').then((interception) => {
        expect(interception.request.headers).to.include({
          authorization: `Bearer ${mockToken}`
        });
        expect(interception.request.body).to.deep.equal({
          nomeItem: 'Item Atualizado',
          quantidade: 2
        });
      });
    });
  });

  it('should delete an item', () => {
    cy.intercept('DELETE', `${API_URL}/1/item/1`, (req) => {
      expect(req.headers).to.have.property('authorization', `Bearer ${mockToken}`);
      req.reply({
        statusCode: 200
      });
    }).as('deleteItemRequest');

    cy.wrap(deleteItem(1, 1)).then(() => {
      cy.get('@deleteItemRequest').its('request.headers')
        .should('include', {
          authorization: `Bearer ${mockToken}`
        });
    });
  });

  it('should handle error when getting items', () => {
    const errorMessage = 'Failed to fetch items';

    cy.intercept('GET', '**/api/lista/1/item', {
      statusCode: 400,
      body: errorMessage
    });

    cy.wrap(getItens(1))
      .should('be.rejected')
      .then((error) => {
        expect(error.message).to.equal(errorMessage);
      });
  });

  it('should handle error when adding item', () => {
    const errorMessage = 'Failed to add item';

    cy.intercept('POST', '**/api/lista/1/item', {
      statusCode: 400,
      body: errorMessage
    });

    cy.wrap(addItem(1, 'Novo Item', 1))
      .should('be.rejected')
      .then((error) => {
        expect(error.message).to.equal(errorMessage);
      });
  });
});
