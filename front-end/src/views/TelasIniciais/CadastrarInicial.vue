<template>
  <div class="about">
    <LogoInicio/>
    <v-form>
      <v-text-field label="Email" class="input mt-8"></v-text-field>
      <v-text-field label="Usuário" class="input"></v-text-field>
      <v-text-field label="Senha" class="input" type="password"></v-text-field>
      <v-text-field label="Confirmar senha" class="input" type="password"></v-text-field>
    </v-form>
    <v-btn class="btnInicial mt-8" @click="cadastrar()">Cadastrar</v-btn>
    <p class="mt-4">Já possui uma conta? <RouterLink to="/login" class="ml-2 textoLink">Entre</RouterLink> </p>
  </div>
</template>

<script>
  import LogoInicio from '@/components/LogoInicio.vue';

  export default {
    components: {
      LogoInicio
    },

    methods: {
      cadastrar () {
        this.$router.push('/home')
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

.input {
  width: 75vw;
  font-size: 16px;
}
</style>
