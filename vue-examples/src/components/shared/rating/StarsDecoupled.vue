<template>
  <div class="rating-container">
      <slot name="filled" v-bind="!!idx|| null" v-for="idx in completeRange" />
      <slot name="half" v-bind="!!idx|| null"  v-for="idx in halfRange" />
      <slot name="empty" v-bind="!!idx|| null"  v-for="idx in totalRange" />
  </div>
</template>

<script>
const range = (start, end, length = end - start + 1) =>
  Array.from({ length }, (_, i) => start + i)

export default {
  name: 'RatingDecoupledComponent',
  props: {
    value: Number,
    total: Number
  },
  data () {
    return {
      completeRange: range(0, this.value - 1),
      halfRange: range(0, this.value % 1 === 0 ? 0 : 1),
      totalRange: range(0, this.total - this.value)
    }
  },
  created: function () {
    if (this.value > this.total) {
      throw new Error('total lower than value')
    }
  }
}
</script>

<style scoped>
.rating-container {
  display:flex;
  justify-content: center;
}
</style>
