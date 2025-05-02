<template>
    <div class="device py-10 px-8">
        <HeaderPrincipal />
        <div class="text-center mb-6">
            <input type="text" :value="lista?.title || 'Digite o nome da lista'" class="inputListName text-center">
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
            <v-btn class="btnSalvar" @click="saveLista()">Salvar</v-btn>
            <img src="../../assets/imgs/shareIcon.svg" alt="Botão de Compartilhar">
        </div>
    </div>
</template>

<script>
import HeaderPrincipal from '@/components/HeaderPrincipal.vue';

export default {
    components: {
        HeaderPrincipal
    },
    data () {
        return {
            lista: {
                title: ''
            },
            itens: [
                {id: 0, text: 'Item 1', quantidade: 1},
                {id: 1, text: 'Item 2', quantidade: 10},
                {id: 2, text: 'Item 3', quantidade: 500},
                {id: 0, text: 'Item 1', quantidade: 1},
                {id: 1, text: 'Item 2', quantidade: 10},
                {id: 2, text: 'Item 3', quantidade: 500},
                {id: 0, text: 'Item 1', quantidade: 1},
                {id: 1, text: 'Item 2', quantidade: 10},
                {id: 2, text: 'Item 3', quantidade: 500},
            ]
        }
    },
    methods: {
        removeItem () {
            console.log('removeu item')
        },
        saveLista () {
            console.log('salvar lista')
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