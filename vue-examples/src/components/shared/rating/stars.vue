<template>
  <div class="rating-container">
      <font-awesome-icon icon="star"  v-for="idx in completeRange" v-bind:key="idx + 'st'" />
      <font-awesome-icon :icon="['fas', 'star-half-alt']"  v-for="idx in halfRange" v-bind:key="idx + 'stt'" />
      <font-awesome-icon :icon="['far', 'star']"  v-for="idx in totalRange" v-bind:key="idx" />
  </div>
</template>

<script>
const range = (start, end, length = end - start + 1) =>
  Array.from({ length }, (_, i) => start + i)

export default {
  name: 'RatingComponent',
  props: {
    value: Number,
    total: Number
  },
  data: function () {
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
