<template>
  <div class="about">
    <LogoInicio/>
    <v-form>
      <v-text-field v-model="email" label="Usuário / email" class="input mt-8"></v-text-field>
      <v-text-field v-model="senha" label="Senha" class="input" type="password"></v-text-field>
    </v-form>
    <v-btn class="btnInicial mt-8" @click="logar()">Entrar</v-btn>
    <p class="mt-4">Não possui uma conta? <RouterLink to="/cadastrar" class="ml-2 textoLink">Cadastre-se</RouterLink> </p>
  </div>
</template>

<script>
  import LogoInicio from '@/components/LogoInicio.vue';
  import { login } from '@/services/auth.js';

  export default {
    components: { LogoInicio },
    data() {
      return {
        email: '',
        senha: ''
      }
    },
    methods: {
      async logar () {
        try {
          await login(this.email, this.senha);
          this.$router.push('/home');
        } catch (err) {
          alert('Erro ao fazer login: ' + err.message);
        }
      }
    }
  }
</script>

<style scoped>
.about {
  min-height: 75dvh;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-direction: column;
}

.input {
  width: 75vw;
  font-size: 16px;
}
</style>
