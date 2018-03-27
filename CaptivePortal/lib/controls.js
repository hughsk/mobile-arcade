var Touches = require('touches')

module.exports = Controls

function Controls (client, root) {
  button(root.querySelector('[name=direction]'), sendForType('tilt'))
  // button(root.querySelector('[name=action]'), sendForType('action'))

  function sendForType (type) {
    return function (enabled, x, y) {
      // semi-normalize (limit length of vector to 1)
      var l = Math.sqrt(x * x + y * y)
      if (l > 1) {
        x /= l
        y /= l
      }

      client.emit('client:input', {
        id: client.id,
        type: type,
        xInput: x,
        yInput: -y
      })
    }
  }

  function button (node, signal) {
    var bounds = node.getBoundingClientRect()
    var pressed = {}
    var touches = Touches(node.parentNode, {
      target: node,
      filtered: true,
      preventSimulated: false
    }).on('start', onTouchStart)
      .on('move', onTouchMove)
      .on('end', onTouchEnd)

    function onTouchStart (e, pos) {
      if (e.target !== node) return
      e.preventDefault()
      pressed = true
      bounds = node.getBoundingClientRect()
      onTouchMove(e, pos)
      node.classList.add('active')
    }

    function onTouchMove (e, pos) {
      if (!pressed) return
      var x = (pos[0] / bounds.width - 0.5) * 2
      var y = (pos[1] / bounds.height - 0.5) * 2
      signal(true, x, y)
    }

    function onTouchEnd (e) {
      if (!pressed) return
      node.classList.remove('active')
      pressed = false
      signal(false, 0, 0)
    }
  }
}