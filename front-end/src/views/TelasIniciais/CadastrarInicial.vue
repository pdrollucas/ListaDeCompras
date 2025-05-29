<template>
  <div class="containerInicial">
    <v-snackbar
      v-model="snackbar.show"
      :color="snackbar.color"
      :timeout="4000"
      location="top"
    >
      {{ snackbar.text }}
    </v-snackbar>
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
        showConfirmPassword: false,
        snackbar: {
          show: false,
          text: '',
          color: 'success'
        },
        errors: {
          email: false,
          nomeUsuario: false,
          senha: false,
          confirmarSenha: false
        }
      }
    },
    methods: {
      validateForm() {
        this.errors = {
          email: !this.email || !this.email.includes('@'),
          nomeUsuario: !this.nomeUsuario || this.nomeUsuario.length < 3,
          senha: !this.senha || this.senha.length < 6,
          confirmarSenha: this.senha !== this.confirmarSenha
        }

        if (Object.values(this.errors).some(error => error)) {
          let errorMessage = '';
          if (this.errors.email) errorMessage = 'Email inválido';
          else if (this.errors.nomeUsuario) errorMessage = 'Nome de usuário deve ter pelo menos 3 caracteres';
          else if (this.errors.senha) errorMessage = 'Senha deve ter pelo menos 6 caracteres';
          else if (this.errors.confirmarSenha) errorMessage = 'As senhas não coincidem';

          this.snackbar = {
            show: true,
            text: errorMessage,
            color: 'error'
          };
          return false;
        }
        return true;
      },

      async cadastrar() {
        if (!this.validateForm()) return;

        try {
          await register(this.email, this.senha, this.nomeUsuario);
          const userStore = useUserStore()
          userStore.setToken(localStorage.getItem('token'))
          
          this.snackbar = {
            show: true,
            text: 'Cadastro realizado com sucesso!',
            color: 'success'
          };
          
          this.$router.push('/home');
        } catch (err) {
          this.snackbar = {
            show: true,
            text: err.response?.data || 'Erro ao cadastrar usuário',
            color: 'error'
          };
        }
      }
    }
  }
</script>
