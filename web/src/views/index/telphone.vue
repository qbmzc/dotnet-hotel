<script setup>
defineProps({
  phonePrefix: String,
  phoneNumber: String
})

defineEmits(['update:phonePrefix', 'update:phoneNumber'])
</script>

<template>
  <input
    type="text"
    :value="phonePrefix"
    @input="$emit('update:phonePrefix', $event.target.value)"
  />
  <span>-</span>
  <input
    type="text"
    :value="phoneNumber"
    @input="$emit('update:phoneNumber', $event.target.value)"
  />
</template>
