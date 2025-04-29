<template>
  <div class="about">
    <LogoInicio/>
    <v-form>
      <v-text-field label="Usuário / email" class="input mt-8"></v-text-field>
      <v-text-field label="Senha" class="input" type="password"></v-text-field>
    </v-form>
    <v-btn class="btnInicial mt-8" @click="logar()">Entrar</v-btn>
    <p class="mt-4">Não tem uma conta? <RouterLink to="/cadastrar" class="ml-2 textoLink">Cadastre-se</RouterLink> </p>
  </div>
</template>

<script>
  import LogoInicio from '@/components/LogoInicio.vue';

  export default {
    components: {
      LogoInicio
    },

    methods: {
      logar () {
        this.$router.push('/home')
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
