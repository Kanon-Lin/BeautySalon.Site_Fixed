
// navbar共用-start
document.addEventListener('DOMContentLoaded', function() {
    fetch('./Htmls/navbar.html')
      .then(response => {
        if (!response.ok) {
          throw new Error('Network response was not ok');
        }
        return response.text();
      })
      .then(data => {
        document.getElementById('navbar').innerHTML = data;
      })
      .catch(error => {
        console.error('There was a problem with the fetch operation:', error);
        document.getElementById('navbar').innerHTML = 'Failed to load navbar.';
      });
  });
//navbar共用-end
