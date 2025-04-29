<template>
  <RouterView />
  <header>
    <div>
      <div class="about">
        <LogoInicio/>
      </div>
      <nav class="btns">
        <RouterLink to="/login" class="mr-2 btn btn-1">Entrar</RouterLink>
        <RouterLink to="/cadastrar" class="ml-2 btn btn-2">Cadastrar</RouterLink>
      </nav>
    </div>
  </header>
</template>

<script setup>
  import LogoInicio from '@/components/LogoInicio.vue';
  import { RouterLink, RouterView } from 'vue-router'
</script>

<style scoped>
.about {
  height: 75dvh;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-direction: column;
}

.btns {
  bottom: 0;
  position: absolute;
  padding-bottom: 10dvh;
  display: flex;
  justify-content: center;
  align-items: center;
  width: 100vw;
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
