const sliderrail_element = document.getElementById("sliderrail")
let amount_of_slides = document.querySelectorAll("#sliderrail div").length
let current_slide = 1

setInterval(() => {
    current_slide++
    if (current_slide > amount_of_slides) {current_slide = 1 }
    sliderrail_element.style.left = "-" + ((current_slide-1)*600)+"px"
},3000)

console.log(current_slide)