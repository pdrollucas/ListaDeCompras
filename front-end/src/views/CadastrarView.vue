<template>
  <div class="about">
    <RouterLink to="/">
      <img src="../assets/imgs/Cart.svg" alt="Logo">
    </RouterLink>
    <h2>Gerenciador de Listas</h2>
    <v-form>
      <v-text-field label="Email" class="input mt-8"></v-text-field>
      <v-text-field label="Usuário" class="input"></v-text-field>
      <v-text-field label="Senha" class="input" type="password"></v-text-field>
      <v-text-field label="Confirmar senha" class="input" type="password"></v-text-field>
    </v-form>
    <v-btn class="btn mt-8">Cadastrar</v-btn>
    <p class="mt-4">Já possui uma conta? <RouterLink to="/login" class="ml-2">Entre</RouterLink> </p>
  </div>
</template>

<style scoped>
* {
  font-family: "Roboto", sans-serif !important;
}

img {
  height: 20vh;
}

.about {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-direction: column;
}

h2 {
  font-size: 18px;
}

.input {
  width: 75vw;
  font-size: 16px;
}

.btn {
  width: 75vw;
  background-color: #0E8AFF;
  color: white;
}
</style>
