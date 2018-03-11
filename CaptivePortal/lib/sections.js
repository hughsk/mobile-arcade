module.exports = Sections

function Sections (targetSection) {
  const sections = document.querySelectorAll('section[name]')

  changeSection(targetSection)
  return changeSection

  function changeSection (target) {
    for (var i = 0; i < sections.length; i++) {
      sections[i].style.display = (
        sections[i].getAttribute('name') === target
      ) ? null : 'none'
    }
  }
}