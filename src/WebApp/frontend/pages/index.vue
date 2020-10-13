<template>
  <div>
    <Navbar />
    <div>
      <h1>
        Header
      </h1>
    </div>
    <v-row class="purchase-container">
      <v-col
        cols="12"
        lg="6"
        class="offset-lg-3"
      >
        <section class="form-container">
          <ProductForm :product="selectedProduct" @onSave="addProduct($event)" @onEdit="editProduct($event)" />
        </section>
      </v-col>
      <v-col
        cols="12"
        lg="8"
        class="offset-lg-2"
      >
        <section class="table-container">
          <Table :headers="headers" :items="products" @onEdit="setProduct($event)" @onDelete="deleteProduct($event)" />
        </section>
      </v-col>
      <v-divider dark />
      <v-col
        cols="12"
        lg="6"
        class="offset-lg-3"
      >
        <section class="form-container">
          <OrderForm @onRequest="requestPurchase($event)" />
        </section>
      </v-col>
    </v-row>
  </div>
</template>

<script lang="ts">
import Vue from 'vue'
import Navbar from '../components/Navbar.vue'
import ProductForm from '../components/ProductForm.vue'
import Table from '../components/Table.vue'
import OrderForm from '../components/OrderForm.vue'
import * as PurchaseAplicationService from '../services/puchase-aplication/purchase-aplication-service'
import { PurchaseAplication } from '../models/purchase-aplication.models'

export default Vue.extend({
  components: {
    Navbar,
    ProductForm,
    OrderForm,
    Table
  },
  data () {
    return {
      products: [] as any[],
      selectedProduct: {} as any,
      headers: [
        { text: 'Enlace', value: 'link' },
        { text: 'Unidades', value: 'units' },
        { text: 'Código de promoción', value: 'promoCode' },
        { text: 'Información', value: 'information' },
        { text: 'Actions', value: 'actions', sortable: false }
      ]
    }
  },
  methods: {
    setProduct (product: any) {
      this.selectedProduct = { ...product }
    },
    addProduct (product: any) {
      this.products.push(product)

      this.selectedProduct = {}
    },
    editProduct (product: any) {
      const productIndex = this.products.findIndex((p: any) => product.id === p.id)
      this.products.splice(productIndex, 1, product)

      this.selectedProduct = {}
    },
    deleteProduct (id: any) {
      const productIndex = this.products.findIndex((p: any) => id === p.id)
      this.products.splice(productIndex, 1)
    },
    requestPurchase (order: any) {
      if (this.products.length > 0) {
        const finalOrder: PurchaseAplication = {
          ...order,
          products: [
            ...this.products.map(({ link, units, promoCode, information }) => ({
              link,
              units,
              promoCode,
              information
            }))
          ]
        }

        PurchaseAplicationService.sendPurchaseAplication({
          axios: this.$axios,
          body: finalOrder
        }).then(() => {
          this.products = []
          this.selectedProduct = {}
        })
      }
    }
  }
})
</script>

<style>

.purchase-container hr {
  flex: 1 1 100%;
  margin: 5vh auto;
  width: 100%;
}

.purchase-container {
  /*background-color: #4a88ca;
  background-color: #2B6CB0;*/
  background-color: #0b1736;
  padding: 3rem;
}

.form-container {
  margin: 0 auto;
}

.table-container {
  margin: 5vh auto;
}

.offset-lg-3 {
  margin-left: 0;
}

.offset-lg-2 {
  margin-left: 0;
}

@media (min-width: 1264px) {
  .offset-lg-3 {
    margin-left: 25%;
  }

  .offset-lg-2 {
    margin-left: 16.6666666667%;
  }

}

</style>
