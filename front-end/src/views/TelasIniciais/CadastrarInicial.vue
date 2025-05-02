<template>
  <div class="about">
    <LogoInicio/>
    <v-form>
      <v-text-field v-model="email" label="Email" class="input mt-8"></v-text-field>
      <v-text-field v-model="nomeUsuario" label="Usuário" class="input"></v-text-field>
      <v-text-field v-model="senha" label="Senha" class="input" type="password"></v-text-field>
      <v-text-field v-model="confirmarSenha" label="Confirmar senha" class="input" type="password"></v-text-field>
    </v-form>
    <v-btn class="btnInicial mt-8" @click="cadastrar()">Cadastrar</v-btn>
    <p class="mt-4">Já possui uma conta? <RouterLink to="/login" class="ml-2 textoLink">Entre</RouterLink> </p>
  </div>
</template>

<script>
  import LogoInicio from '@/components/LogoInicio.vue';
  import { register } from '@/services/auth.js';
  import { useUserStore } from '@/stores/user'

  export default {
    components: { LogoInicio },
    data() {
      return {
        email: '',
        nomeUsuario: '',
        senha: '',
        confirmarSenha: ''
      }
    },
    methods: {
      async cadastrar () {
        if (this.senha !== this.confirmarSenha) {
          alert('As senhas não coincidem!');
          return;
        }
        try {
          await register(this.email, this.senha, this.nomeUsuario);
          const userStore = useUserStore()
          userStore.setToken(localStorage.getItem('token'))
          this.$router.push('/home');
        } catch (err) {
          alert('Erro ao cadastrar: ' + err.message);
        }
      }
    }
  }
</script>

<style scoped>
.about {
  min-height: 100dvh;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-direction: column;
}
</style>
