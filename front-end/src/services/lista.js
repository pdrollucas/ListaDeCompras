const API_URL = "http://localhost:5139/api/Lista";

function getAuthHeader() {
  const token = localStorage.getItem("token");
  return { Authorization: `Bearer ${token}` };
}

export async function getListas() {
  const response = await fetch(API_URL, {
    headers: { ...getAuthHeader() }
  });
  if (!response.ok) throw new Error(await response.text());
  return await response.json();
}

export async function addLista(nomeLista) {
  const response = await fetch(API_URL, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      ...getAuthHeader()
    },
    body: JSON.stringify({ nomeLista })
  });
  if (!response.ok) throw new Error(await response.text());
  return await response.json();
}

export async function updateLista(id, nomeLista) {
  const response = await fetch(`${API_URL}/${id}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
      ...getAuthHeader()
    },
    body: JSON.stringify({ nomeLista })
  });
  if (!response.ok) throw new Error(await response.text());
  return await response.json();
}

export async function deleteLista(id) {
    const response = await fetch(`${API_URL}/${id}`, {
      method: "DELETE",
      headers: { ...getAuthHeader() }
    });
    if (!response.ok) throw new Error(await response.text());
  }