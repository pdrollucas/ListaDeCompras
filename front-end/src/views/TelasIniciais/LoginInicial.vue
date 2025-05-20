<template>
  <div class="containerInicial">
    <LogoInicio/>
    <v-form>
      <v-text-field v-model="email" label="Usuário / email" class="inputInicial mt-8"></v-text-field>
      <v-text-field
        v-model="senha"
        :type="showPassword ? 'text' : 'password'"
        label="Senha"
        class="inputInicial"
        :append-icon="showPassword ? 'mdi-eye' : 'mdi-eye-off'"
        @click:append="showPassword = !showPassword"
      ></v-text-field>
    </v-form>
    <v-btn class="btnInicial mt-8" @click="logar()">Entrar</v-btn>
    <p class="mt-4">Não possui uma conta? <RouterLink to="/cadastrar" class="ml-2 textoLink">Cadastre-se</RouterLink> </p>
    <p class="mt-10"><RouterLink to="/esqueceu-senha" class="textoLink">Esqueci minha senha</RouterLink> </p>
  </div>
</template>

<script>
  import LogoInicio from '@/components/LogoInicio.vue';
  import { login } from '@/services/auth.js';
  import { useUserStore } from '@/stores/user'

  export default {
    components: { LogoInicio },
    data() {
      return {
        email: '',
        senha: '',
        showPassword: false
      }
    },
    methods: {
      async logar () {
        try {
          await login(this.email, this.senha)
          const userStore = useUserStore()
          userStore.setToken(localStorage.getItem('token'))
          this.$router.push('/home')
        } catch (err) {
          alert('Erro ao fazer login: ' + err.message)
        }
      }
    }
  }
</script>
