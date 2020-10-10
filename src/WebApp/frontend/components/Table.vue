<template>
  <v-data-table
    :headers="headers"
    :items="items"
    class="elevation-1"
  >
    <template v-slot:item.actions="{item}">
      <v-icon
        small
        @click="onEdit(item)"
      >
        mdi-pencil
      </v-icon>
      <v-icon
        small
        @click="onDelete(item)"
      >
        mdi-delete
      </v-icon>
    </template>
  </v-data-table>
</template>

<script lang="ts">
import Vue from 'vue'

export default Vue.extend({
  name: 'Table',
  props: {
    headers: {
      required: true,
      type: Array,
      default: () => []
    },
    items: {
      required: true,
      type: Array,
      default: () => []
    }
  },
  data () {
    return {}
  },
  methods: {
    onEdit (item: any) {
      this.$emit('onEdit', item)
    },
    onDelete (item: any) {
      this.$emit('onDelete', item.id)
    }
  }
})
</script>

<style scoped>
</style>
