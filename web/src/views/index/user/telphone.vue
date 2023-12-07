<script setup>
defineProps({
  phonePrefix: String,
  mobile: String
})

// defineEmits(['update:mobile'])
</script>

<template>
  <input maxlength="4" readonly="readonly"
    type="text" required="required"
    :value="phonePrefix"
  /><span>-</span>
  <input required="required"
    type="text" 
    :value="mobile"
   
  />
</template>
