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

window.addEventListener("beforeunload", function (event) {
   //your code goes here on location change 
	console.log(window.location.href);
});

window.addEventListener("load", (event) => {

});


	},
}

