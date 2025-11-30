import { defineStore } from 'pinia'
import { getUserName, getToken, logout as authLogout } from '@/services/auth.js'
import { getListas as apiGetListas } from '@/services/lista.js'

export const useUserStore = defineStore('user', {
  state: () => ({
    token: getToken(),
    usuario: getUserName(),
    listas: []
  }),
  actions: {
    setToken(token) {
      this.token = token
      this.usuario = getUserName()
    },
    clearUser() {
      this.token = null
      this.usuario = null
      this.listas = []
      authLogout()
    },
    setListas(listas) {
      this.listas = listas
    },
    async fetchListas() {
      this.listas = await apiGetListas();
    }
  }
})