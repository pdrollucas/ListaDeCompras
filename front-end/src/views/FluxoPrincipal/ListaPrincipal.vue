<template>
    <div class="device py-10 px-8">
        <HeaderPrincipal :telaExpandida="telaExpandida">
            <template #right>
                <v-icon v-if="telaExpandida" icon @click="toggleExpandir">mdi-arrow-collapse</v-icon>
            </template>
        </HeaderPrincipal>
        <div v-if="!telaExpandida" class="text-center mb-6">
            <input type="text" v-model="nomeLista" class="inputListName text-center">
            <v-form>
                <v-text-field label="Item" class="input mt-8" v-model="nomeItem"></v-text-field>
                <v-text-field label="Quantidade" class="input" v-model="quantidade"></v-text-field>
            </v-form>
            <v-btn class="btnAddItem" @click="addItem()">Adicionar</v-btn>
        </div>
        <div :class="['itens', telaExpandida ? 'itens-telaExpandida' : '']">
          <span class="w-100 mt-4 pr-2 expandirBtn">
            <v-icon v-if="!telaExpandida" @click="toggleExpandir" small>mdi-arrow-expand</v-icon>
          </span>
          <div class="py-10">
            <ul v-for="item in itens" :key="item.idItem">
                <li class="item py-4 px-4">
                    <span>{{ item.quantidade }} - {{ item.nomeItem }}</span>
                    <span @click="removeItem(item.idItem)">x</span>
                </li>
            </ul>
          </div>
        </div>
        <div
            v-if="!telaExpandida"
            class="w-100 d-flex justify-content-between align-items-center mt-10"
        >
            <span></span>
            <v-btn class="btnSalvar" @click="saveLista()">Salvar</v-btn>
            <img src="../../assets/imgs/shareIcon.svg" alt="Botão de Compartilhar">
        </div>
    </div>
</template>

<script>
import HeaderPrincipal from '@/components/HeaderPrincipal.vue'
import { useUserStore } from '@/stores/user'
import { updateLista } from '@/services/lista.js'
import { getItens, addItem, deleteItem } from '@/services/item.js'

export default {
  components: { HeaderPrincipal },
  data() {
    return {
      nomeLista: 'Digite o nome da lista',
      idLista: null,
      itens: [],
      nomeItem: '',
      quantidade: '',
      telaExpandida: false,
      userStore: useUserStore()
    }
  },
  async mounted() {
    this.idLista = this.$route.params.id

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
    this.itens = await getItens(this.idLista)
  },
  methods: {
    async saveLista() {
        try {
        await updateLista(this.idLista, this.nomeLista)
        await this.userStore.fetchListas()
        this.$router.push('/home')
        } catch (err) {
        alert('Erro ao salvar lista: ' + err.message)
        }
    },
    async addItem() {
      try {
        await addItem(this.idLista, this.nomeItem, this.quantidade)
        this.itens = await getItens(this.idLista)
        this.nomeItem = ''
        this.quantidade = ''
      } catch (err) {
        alert('Erro ao adicionar item: ' + err.message)
      }
    },
    async removeItem(idItem) {
      try {
        await deleteItem(this.idLista, idItem)
        this.itens = await getItens(this.idLista)
      } catch (err) {
        alert('Erro ao remover item: ' + err.message)
      }
    },
    toggleExpandir() {
      this.telaExpandida = !this.telaExpandida
    },
    goHome() {
      this.$router.push('/home')
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
    height: 50dvh;
    overflow-y: auto;
    background-color: #DEDEDE;
    border-top: 1px solid gray;
    border-bottom: 1px solid gray;
    transition: height 0.3s;
}

.itens-telaExpandida {
    height: 80dvh !important;
}

.item {
    background-color: #FDFDFD;
    width: 75dvw;
    display: flex;
    justify-content: space-between;
    align-items: center;
}

.expandirBtn {
  display: flex;
  justify-content: flex-end;
  position: absolute;
  color: gray;
}

.btnSalvar {
  width: 50dvw;
  background-color: #0E8AFF !important;
  color: white !important;
}
</style>