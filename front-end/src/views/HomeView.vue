<script setup>
import { RouterLink, RouterView } from 'vue-router'
</script>

<template>
  <RouterView />
  <header>
    <div>
      <div class="about">
        <img src="../assets/imgs/Cart.svg" alt="Logo">
        <h2>Gerenciador de Listas</h2>
      </div>
      <nav class="btns">
        <RouterLink to="/login" class="mr-2 btn btn-1">Entrar</RouterLink>
        <RouterLink to="/cadastrar" class="ml-2 btn btn-2">Cadastrar</RouterLink>
      </nav>
    </div>
  </header>
</template>

<style scoped>
* {
  font-family: "Roboto", sans-serif !important;
}

img {
  height: 25vh;
}

.about {
  height: 75vh;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-direction: column;
}

h2 {
  font-size: 18px;
  font-family: "Roboto", sans-serif !important;
}

.btns {
  bottom: 0;
  position: absolute;
  padding-bottom: 10vh;
  display: flex;
  justify-content: center;
  align-items: center;
  width: 100vw;
  font-family: "Roboto", sans-serif !important;
}

.btn {
  width: 40vw;
  border-radius: 5px;
  padding: 3vw;
}

.btn-1 {
  border: 2px solid #0E8AFF;
  background-color: white;
  color: #0E8AFF;
}

.btn-2 {
  background-color: #0E8AFF;
  color: white;
}
</style>
