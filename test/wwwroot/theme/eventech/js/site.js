// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


var makeMap = function (lat,lng) {
    var mymap = L.map('mapid').setView([lat, lng ], 13);
    L.tileLayer('https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token={accessToken}',
        {
            attribution: '',// 'Map data &copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors, <a href="http://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="http://mapbox.com">Mapbox</a>',
            subdomains: ['a', 'b', 'c', 'd'],
            maxZoom: 23,
            id: 'mapbox/streets-v11',
            accessToken: 'pk.eyJ1IjoidG9oZWxsaGkiLCJhIjoiY2trcnJlYnEzMHdjYjMwcW4weXhyOHhneSJ9.1V-Cl1NOv2E9XaLyZqNPtw'
        }).addTo(mymap);

    return mymap;
}