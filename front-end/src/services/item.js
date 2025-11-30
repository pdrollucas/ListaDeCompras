const API_URL = "http://localhost:5139/api/lista";

function getAuthHeader() {
  const token = localStorage.getItem("token");
  return { Authorization: `Bearer ${token}` };
}

export async function getItens(listaId) {
  const response = await fetch(`${API_URL}/${listaId}/item`, {
    headers: { ...getAuthHeader() }
  });
  if (!response.ok) throw new Error(await response.text());
  return await response.json();
}

export async function addItem(listaId, nomeItem, quantidade) {
  const response = await fetch(`${API_URL}/${listaId}/item`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      ...getAuthHeader()
    },
    body: JSON.stringify({ nomeItem, quantidade })
  });
  if (!response.ok) throw new Error(await response.text());
  return await response.json();
}

export async function updateItem(listaId, idItem, nomeItem, quantidade) {
  const response = await fetch(`${API_URL}/${listaId}/item/${idItem}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
      ...getAuthHeader()
    },
    body: JSON.stringify({ nomeItem, quantidade })
  });
  if (!response.ok) throw new Error(await response.text());
  return await response.json();
}

export async function deleteItem(listaId, idItem) {
  const response = await fetch(`${API_URL}/${listaId}/item/${idItem}`, {
    method: "DELETE",
    headers: { ...getAuthHeader() }
  });
  if (!response.ok) throw new Error(await response.text());
}
