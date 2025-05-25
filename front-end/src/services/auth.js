const API_URL = "http://localhost:5139/api/Auth";

export async function login(email, senha) {
  const response = await fetch(`${API_URL}/login`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ email, senha }),
  });

  if (!response.ok) {
    const error = await response.text();
    throw new Error(error);
  }

  const data = await response.json();
  localStorage.setItem("token", data.token);
  return data.token;
}

export async function register(email, senha, nomeUsuario) {
  const response = await fetch(`${API_URL}/register`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ email, senha, nomeUsuario }),
  });

  if (!response.ok) {
    const error = await response.text();
    throw new Error(error);
  }

  const data = await response.json();
  localStorage.setItem("token", data.token);
  return data.token;
}

export function logout() {
  localStorage.removeItem("token");
}

export function getToken() {
  return localStorage.getItem("token");
}

export function isAuthenticated() {
  return !!localStorage.getItem("token");
}

export async function solicitarRecuperacaoSenha(email) {
  const response = await fetch(`${API_URL}/solicitar-recuperacao`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ email }),
  });

  if (!response.ok) {
    const error = await response.text();
    throw new Error(error);
  }

  return await response.json();
}

export async function validarCodigoEAtualizarSenha(email, codigo, novaSenha) {
  const response = await fetch(`${API_URL}/validar-codigo`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ email, codigo, novaSenha }),
  });

  if (!response.ok) {
    const error = await response.text();
    throw new Error(error);
  }

  const data = await response.json();
  localStorage.setItem("token", data.token);
  return data.token;
}

// Função para decodificar o token JWT
function parseJwt(token) {
  if (!token) return null;
  try {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(atob(base64).split('').map(function(c) {
      return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));
    return JSON.parse(jsonPayload);
  } catch (e) {
    return console.log(e);
  }
}

// Função para obter o nome do usuário logado
export function getUserName() {
  const token = localStorage.getItem("token");
  const payload = parseJwt(token);
  return payload ? payload.unique_name : null;
}