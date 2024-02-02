export default {
	defaultTheme: 'light',

iconLinks: [
    {
      icon: 'github',
      href: 'https://github.com/dotnet/docfx',
      title: 'GitHub'
    },
    {
      icon: 'twitter',
      href: 'https://twitter.com',
      title: 'Twitter'
    }
  ],
	start: () => {
	    // Startup script goes here

localStorage.setItem('theme', 'light');

window.addEventListener("load", (event) => {

const testElements = document.getElementsByClassName("icons");
const testForms = Array.prototype.filter.call(
  testElements,
  (testElement) => testElement.nodeName === "FORM",
);
testForms.forEach((element) => element.innerHTML='');

});


	},
}