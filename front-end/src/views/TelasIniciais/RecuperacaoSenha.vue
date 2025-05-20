<template>
  <div class="containerInicial">
    <LogoInicio/>
    <v-form>
      <v-text-field v-model="email" label="Código recebido no e-mail" class="inputInicial mt-8"></v-text-field>
      <v-text-field
        v-model="senha"
        :type="showPassword ? 'text' : 'password'"
        label="Nova senha"
        class="inputInicial"
        :append-icon="showPassword ? 'mdi-eye' : 'mdi-eye-off'"
        @click:append="showPassword = !showPassword"
      ></v-text-field>
      <v-text-field
        v-model="confirmarSenha"
        :type="showConfirmPassword ? 'text' : 'password'"
        label="Confirmar senha"
        class="inputInicial"
        :append-icon="showConfirmPassword ? 'mdi-eye' : 'mdi-eye-off'"
        @click:append="showConfirmPassword = !showConfirmPassword"
      ></v-text-field>
    </v-form>
    <v-btn class="btnInicial mt-8" @click="atualizarSenha()">Atualizar senha</v-btn>
    <p class="mt-4 textoLink" @click="reenviarCodigo()">Reenviar código</p>
  </div>
</template>

<script>
import LogoInicio from '@/components/LogoInicio.vue';

export default {
    components: {
        LogoInicio
    },
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
      async atualizarSenha () {
        if (this.senha !== this.confirmarSenha) {
          alert('As senhas não coincidem!');
          return;
        }
        try {
          // await
          // atualizar a senha do usuário
          // setar o token jwt
          // this.$router.push('/home')
        } catch (err) {
          alert('Erro ao atualizar senha: ' + err.message);
        }
      },
      async reenviarCodigo () {
        //
      }
    }
}
</script>