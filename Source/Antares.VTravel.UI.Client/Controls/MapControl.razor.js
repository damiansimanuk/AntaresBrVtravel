

export function InitializeMap() {
    window.map = L.map('map').setView([-22.9295678, -0.09], 13);

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
    }).addTo(map);
}

export function AddMark(options = {
    lat: -22.92955,
    long: -45.44939,
    fillColor: '#ff003399',
    circleColor: '#00ffFF99'
}) {

    var myIcon = L.divIcon({
        className: 'location-pin',
        html: `
                <div style="position: relative; 
                    filter: drop-shadow(0 0 0.2rem rgba(0, 0, 0, 0.6)) drop-shadow(0.1rem 0.1rem 0.2rem rgba(0, 0, 0, 0.7)); ">
                <div style="position: relative; top: -0.69rem; left: 0.23rem; width: 1.8rem;">
                    <svg viewBox="6 2 12 20" style="aspect-ratio: 12/17;">
                        <path d='M12 2c-3.87 0-7 3.13-7 7 0 5.25 7 13 7 13s7-7.75 7-13c0-3.87-3.13-7-7-7zm0'
                            style="color: ${options.fillColor}; fill:currentColor" />
                        <path d='M12 10.5c-1.38 0-2.5-1.12-2.5-2.5s1.12-2.5 2.5-2.5 2.5 1.12 2.5 2.5-1.12 2.5-2.5 2.5z'
                            style="color: ${options.circleColor}; fill:currentColor; stroke: currentColor; stroke-width: 0;" />
                    </svg>
                </div>
                </div>
        `,
        iconSize: [30, 30],
        iconAnchor: [18, 30]
    });

    const marker = L.marker([options.lat, options.long], { icon: myIcon })
        .addTo(map)
        .bindPopup('<b>Hello world!</b><br />I am a popup.');
} 
