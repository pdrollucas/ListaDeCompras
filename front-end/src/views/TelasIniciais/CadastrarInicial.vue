<template>
  <div class="containerInicial">
    <LogoInicio/>
    <v-form>
      <v-text-field v-model="email" label="Email" class="input mt-8" data-cy="register-email"></v-text-field>
      <v-text-field v-model="nomeUsuario" label="Usuário" class="input" data-cy="register-name"></v-text-field>
      <v-text-field
        v-model="senha"
        :type="showPassword ? 'text' : 'password'"
        label="Senha"
        class="input"
        :append-icon="showPassword ? 'mdi-eye' : 'mdi-eye-off'"
        @click:append="showPassword = !showPassword"
        data-cy="register-password"
      ></v-text-field>
      <v-text-field
        v-model="confirmarSenha"
        :type="showConfirmPassword ? 'text' : 'password'"
        label="Confirmar senha"
        class="input"
        :append-icon="showConfirmPassword ? 'mdi-eye' : 'mdi-eye-off'"
        @click:append="showConfirmPassword = !showConfirmPassword"
        data-cy="register-confirm-password"
      ></v-text-field>
    </v-form>
    <v-btn class="btnInicial mt-8" @click="cadastrar()" data-cy="register-submit">Cadastrar</v-btn>
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
        confirmarSenha: '',
        showPassword: false,
        showConfirmPassword: false
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
