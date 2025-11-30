<template>
  <div class="containerInicial">
    <v-snackbar
      v-model="snackbar.show"
      :color="snackbar.color"
      location="top"
      :timeout="3000"
    >
      {{ snackbar.text }}
    </v-snackbar>
    <LogoInicio/>
    <p class="w-75 text-center mt-10">Digite seu e-mail para receber um código de recuperação de senha em sua caixa de entrada.</p>
    <v-form ref="form" v-model="valid">
      <v-text-field
        v-model="email"
        label="E-mail"
        class="inputInicial mt-8"
        :rules="emailRules"
        required
      ></v-text-field>
    </v-form>
    <v-btn
      class="btnInicial mt-8"
      @click="recuperarSenha()"
      :loading="loading"
      :disabled="!valid || loading"
    >Recuperar</v-btn>
    <p class="mt-4">Não possui uma conta? <RouterLink to="/cadastrar" class="ml-2 textoLink">Cadastre-se</RouterLink> </p>
  </div>
</template>

<script>
import LogoInicio from '@/components/LogoInicio.vue';
import { solicitarRecuperacaoSenha } from '@/services/auth';

export default {
  components: {
    LogoInicio
  },
  data() {
    return {
      valid: false,
      loading: false,
      email: '',
      emailRules: [
        v => !!v || 'E-mail é obrigatório',
        v => /.+@.+\..+/.test(v) || 'E-mail deve ser válido'
      ],
      snackbar: {
        show: false,
        text: '',
        color: ''
      }
    }
  },
  methods: {
    async recuperarSenha() {
      if (!this.$refs.form.validate()) return;

      this.loading = true;
      try {
        await solicitarRecuperacaoSenha(this.email);
        this.snackbar = {
          show: true,
          text: 'Código de recuperação enviado com sucesso! Verifique seu e-mail.',
          color: 'success'
        };
        localStorage.setItem('recovery_email', this.email); // Salvar o email para a próxima tela
        this.$router.push('/recuperacao-senha');
      } catch (error) {
        this.snackbar = {
          show: true,
          text: error.response?.data || 'Erro ao solicitar recuperação de senha',
          color: 'error'
        };
      } finally {
        this.loading = false;
      }
    }
  }
}
</script>