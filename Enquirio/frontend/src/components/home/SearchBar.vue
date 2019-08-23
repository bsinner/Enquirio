<template>
    <div class="mt-2 form-group row justify-content-center">
        
        <label for="zip" class="col-sm-1 col-form-label px-0 pt-1 text-right">
            Zip Code
        </label>
        <div class="col-sm-3">
            <input type="text" id="zip" class="form-control form-control-sm" />
        </div>

        <label for="radius" class="col-sm-1 col-form-label px-0 pt-1 text-right">Within</label>
        <div class="col-sm-2">
            <select type="text" id="radius" class="form-control form-control-sm">
                <option 
                    v-for="(distance, index) in distances" 
                    v-bind:key="index"
                    v-bind:value="distance">
                    {{ distance }} miles
                </option>
                <option value="any">Anywhere</option>
            </select>
        </div>

    </div>
</template>

<script>
export default {
    data() {
        return { distances: [5, 10, 25, 50, 100, 150, 250] }
    }
}
</script>