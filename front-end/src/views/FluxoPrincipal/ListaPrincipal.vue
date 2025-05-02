<template>
    <div class="device py-10 px-8">
        <HeaderPrincipal />
        <div class="text-center mb-6">
            <input type="text" v-model="nomeLista" class="inputListName text-center">
            <v-form>
                <v-text-field label="Item" class="input mt-8"></v-text-field>
                <v-text-field label="Quantidade" class="input"></v-text-field>
            </v-form>
            <v-btn class="btnAddItem">Adicionar</v-btn>
        </div>
        <div class="itens py-10 px-10">
            <ul v-for="item in itens" :key="item.id">
                <li class="item py-4 px-4">
                    <span>{{ item.quantidade }} - {{ item.text }}</span>
                    <span @click="removeItem()">x</span>
                </li>
            </ul>
        </div>
        <div class="w-100 d-flex justify-content-between align-items-center mt-10">
            <span></span>
            <v-btn class="btnSalvar" @click="saveLista()">{{ isEdicao ? 'Salvar' : 'Criar' }}</v-btn>
            <img src="../../assets/imgs/shareIcon.svg" alt="Botão de Compartilhar">
        </div>
    </div>
</template>

<script>
import HeaderPrincipal from '@/components/HeaderPrincipal.vue'
import { useUserStore } from '@/stores/user'
import { addLista, updateLista } from '@/services/lista.js'

export default {
  components: { HeaderPrincipal },
  data() {
    return {
      nomeLista: 'Digite o nome da lista',
      idLista: null,
      isEdicao: false,
      userStore: useUserStore()
    }
  },
  async mounted() {
    this.idLista = this.$route.params.id
    this.isEdicao = !!this.idLista

    if (this.isEdicao) {
      // Garante que as listas estão carregadas
      if (!this.userStore.listas.length) {
        await this.userStore.fetchListas()
      }
      const lista = this.userStore.listas.find(l => l.idLista == this.idLista)
      if (!lista) {
        alert('Lista não encontrada ou não pertence a você!')
        this.$router.push('/home')
        return
      }
      this.nomeLista = lista.nomeLista
    }
    // Se não for edição, nomeLista já fica vazio para criar nova lista
  },
  methods: {
    async saveLista() {
      try {
        if (this.isEdicao) {
          await updateLista(this.idLista, this.nomeLista)
        } else {
          await addLista(this.nomeLista)
        }
        await this.userStore.fetchListas()
        this.$router.push('/home')
      } catch (err) {
        alert('Erro ao salvar lista: ' + err.message)
      }
    }
  }
}
</script>

<style scoped>
.inputListName {
    font-size: 20px;
    font-family: "Roboto", sans-serif !important;
}

.btnAddItem {
    width: 50vw;
    background-color: #79B5EE !important;
    color: white !important;
}

.itens {
    display: flex;
    flex-direction: column;
    align-items: center;
    width: 100dvw;
    height: 40dvh;
    overflow-y: auto;
    background-color: #DEDEDE;
    border-top: 1px solid gray;
    border-bottom: 1px solid gray;
}

.item {
    background-color: #FDFDFD;
    width: 75dvw;
    display: flex;
    justify-content: space-between;
    align-items: center;
}

.btnSalvar {
  width: 50dvw;
  background-color: #0E8AFF !important;
  color: white !important;
}
</style>