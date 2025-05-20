<template>
  <div class="containerInicial">
    <LogoInicio/>
    <p class="w-75 text-center mt-10">Digite seu nome de usuário ou e-mail para receber um link de recuperação de senha em sua caixa de entrada.</p>
    <v-form>
      <v-text-field v-model="email" label="Usuário / email" class="inputInicial mt-8"></v-text-field>
    </v-form>
    <v-btn class="btnInicial mt-8" @click="recuperarSenha()">Recuperar</v-btn>
    <p class="mt-4">Não possui uma conta? <RouterLink to="/cadastrar" class="ml-2 textoLink">Cadastre-se</RouterLink> </p>
  </div>
</template>

<script>
  import LogoInicio from '@/components/LogoInicio.vue';

  export default {
    components: {
        LogoInicio
    },
    data () {
        return {
            email: ''
        }
    },
    methods: {
        recuperarSenha () {
            // verificar se o email existe nos usuarios
            // enviar o código ao email
            this.$router.push('/recuperacao-senha')
        }
    }
  }
</script>