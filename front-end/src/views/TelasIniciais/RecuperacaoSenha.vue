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
    <v-form ref="form" v-model="valid">
      <v-text-field
        v-model="codigo"
        label="Código recebido no e-mail"
        class="inputInicial mt-8"
        :rules="codigoRules"
        required
      ></v-text-field>
      <v-text-field
        v-model="senha"
        :type="showPassword ? 'text' : 'password'"
        label="Nova senha"
        class="inputInicial"
        :append-icon="showPassword ? 'mdi-eye' : 'mdi-eye-off'"
        @click:append="showPassword = !showPassword"
        :rules="senhaRules"
        required
      ></v-text-field>
      <v-text-field
        v-model="confirmarSenha"
        :type="showConfirmPassword ? 'text' : 'password'"
        label="Confirmar senha"
        class="inputInicial"
        :append-icon="showConfirmPassword ? 'mdi-eye' : 'mdi-eye-off'"
        @click:append="showConfirmPassword = !showConfirmPassword"
        :rules="confirmarSenhaRules"
        required
      ></v-text-field>
    </v-form>
    <v-btn
      class="btnInicial mt-8"
      @click="atualizarSenha()"
      :loading="loading"
      :disabled="!valid || loading"
    >Atualizar senha</v-btn>
    <p class="mt-4 textoLink" @click="reenviarCodigo()" :class="{ 'disabled-link': reenviarLoading }">
      {{ reenviarLoading ? 'Enviando...' : 'Reenviar código' }}
    </p>
  </div>
</template>

<script>
import LogoInicio from '@/components/LogoInicio.vue';
import { solicitarRecuperacaoSenha, validarCodigoEAtualizarSenha } from '@/services/auth';

export default {
  components: {
    LogoInicio
  },
  data() {
    return {
      valid: false,
      loading: false,
      reenviarLoading: false,
      email: '',
      codigo: '',
      senha: '',
      confirmarSenha: '',
      showPassword: false,
      showConfirmPassword: false,
      codigoRules: [
        v => !!v || 'Código é obrigatório',
        v => v.length === 6 || 'Código deve ter 6 dígitos'
      ],
      senhaRules: [
        v => !!v || 'Senha é obrigatória',
        v => v.length >= 6 || 'Senha deve ter no mínimo 6 caracteres'
      ],
      confirmarSenhaRules: [
        v => !!v || 'Confirmação de senha é obrigatória',
        v => v === this.senha || 'As senhas não coincidem'
      ],
      snackbar: {
        show: false,
        text: '',
        color: ''
      }
    }
  },
  created() {
    // Recuperar o email salvo da tela anterior
    this.email = localStorage.getItem('recovery_email');
    if (!this.email) {
      this.$router.push('/esqueceu-senha');
    }
  },
  methods: {
    async atualizarSenha() {
      if (!this.$refs.form.validate()) return;

      this.loading = true;
      try {
        await validarCodigoEAtualizarSenha(
          this.email,
          this.codigo,
          this.senha
        );

        this.snackbar = {
          show: true,
          text: 'Senha atualizada com sucesso!',
          color: 'success'
        };

        // Limpar o email de recuperação do localStorage
        localStorage.removeItem('recovery_email');

        // Redirecionar para a home após um breve delay
        setTimeout(() => {
          this.$router.push('/home');
        }, 1500);
      } catch (error) {
        this.snackbar = {
          show: true,
          text: error.response?.data || 'Erro ao atualizar senha',
          color: 'error'
        };
      } finally {
        this.loading = false;
      }
    },

    async reenviarCodigo() {
      if (this.reenviarLoading) return;

      this.reenviarLoading = true;
      try {
        await solicitarRecuperacaoSenha(this.email);
        this.snackbar = {
          show: true,
          text: 'Novo código enviado com sucesso!',
          color: 'success'
        };
      } catch (error) {
        this.snackbar = {
          show: true,
          text: error.response?.data || 'Erro ao reenviar código',
          color: 'error'
        };
      } finally {
        this.reenviarLoading = false;
      }
    }
  }
}
</script>

<style scoped>
.disabled-link {
  opacity: 0.6;
  cursor: not-allowed;
}
</style>