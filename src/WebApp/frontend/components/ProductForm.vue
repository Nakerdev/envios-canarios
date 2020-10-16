<template>
  <v-form v-model="isValid">
    <h1 style="color: white">
      Añade un producto
    </h1>

    <v-row>
      <v-col
        cols="12"
      >
        <v-text-field
          v-model="product.link"
          aria-label="Enlace de producto"
          dark
          placeholder="https://www.adidas.es/zapatilla-zx-2k-boost/FV9996.html"
          filled
          required
        />
      </v-col>
    </v-row>
    <v-row>
      <v-col
        cols="12"
        md="6"
      >
        <v-text-field
          v-model="product.units"
          label="Número de unidades"
          dark
          filled
          required
        />
      </v-col>
      <v-col
        cols="12"
        md="6"
      >
        <v-text-field
          v-model="product.promotionCode"
          label="Código promocional"
          dark
          filled
        />
      </v-col>
    </v-row>
    <v-row>
      <v-col
        cols="12"
      >
        <v-textarea
          v-model="product.additionalInformation"
          aria-label="Información adicional del producto"
          placeholder="Talla: M, Color: Rojo, Modelo: Iphone 8, "
          dark
          filled
          required
        />
      </v-col>
    </v-row>
    <v-row class="content-flex-end">
      <v-col
        cols="12"
        md="4"
      >
        <v-btn
          v-if="!product.id"
          aria-label="Añadir producto"
          dark
          outlined
          @click.prevent="saveProduct"
        >
          Añadir producto
        </v-btn>
        <v-btn
          v-if="product.id"
          aria-label="Actualizar producto"
          dark
          outlined
          @click.prevent="editProduct"
        >
          Editar producto
        </v-btn>
      </v-col>
    </v-row>
  </v-form>
</template>

<script lang="ts">
import Vue from 'vue'

export default Vue.extend({
  name: 'ProductForm',
  props: {
    product: {
      required: false,
      type: Object,
      default: () => ({})
    }
  },
  data () {
    return {
      isValid: true
    }
  },
  methods: {
    saveProduct () {
      if (this.isValid) {
        const newProduct = {
          id: Math.floor(Math.random() * (1 - 100)) + 1,
          ...this.product
        }

        this.$emit('onSave', newProduct)
      }
    },
    editProduct () {
      if (this.isValid) {
        const editedProduct = {
          ...this.product
        }

        this.$emit('onEdit', editedProduct)
      }
    }
  }
})
</script>

<style scoped>
  button {
    width: inherit;
  }

  .content-flex-end {
    justify-content: flex-end;
  }
</style>
