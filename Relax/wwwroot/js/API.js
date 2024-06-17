//=============================================MAP Start=================================================================
var directionsService;
var directionsRenderer;
var point1; // 第一個景點
var point2; // 第二個景點
var point3; // 第三個景點

// 初始化地圖
function initMap() {
    // 初始化 DirectionsService 和 DirectionsRenderer
    directionsService = new google.maps.DirectionsService();
    directionsRenderer = new google.maps.DirectionsRenderer();

    // 定義三個景點的經緯度座標
    point1 = new google.maps.LatLng(22.6208746, 120.280509); // 第一個景點 : 駁二藝術特區
    point2 = new google.maps.LatLng(22.6198442, 120.2847017); // 第二個景點 : 高雄流行音樂中心
    point3 = new google.maps.LatLng(22.61875850446501, 120.29203981130068); // 第三個景點：高雄流行音樂中心 珊瑚礁群

    // 設定地圖的初始參數
    var mapOptions = {
        zoom: 14, // 初始縮放級別
        center: point1, // 初始中心位置設定為第一個景點
        scrollwheel: true
    };

    // 創建地圖
    var map = new google.maps.Map(document.getElementById('map'), mapOptions);
    directionsRenderer.setMap(map); // 將 DirectionsRenderer 與地圖關聯

    // 顯示地圖資訊
    displayLocationInfo();
}

// 計算並顯示路線
function calcRoute() {
    // 獲取使用者選擇的旅行模式
    var selectedMode = document.getElementById('mode').value;

    // 設定路線規劃的請求
    var request = {
        origin: point1, // 設定起點為第一個景點
        destination: point3, // 設定終點為第三個景點
        waypoints: [{ location: point2 }], // 加入第二個景點作為途經點
        travelMode: google.maps.TravelMode[selectedMode] // 設定旅行模式
    };

    // 請求路線規劃
    directionsService.route(request, function (response, status) {
        if (status == 'OK') {
            directionsRenderer.setDirections(response); // 將路線顯示在地圖上
        }
    });
}

// 轉換座標為地點資訊並顯示
function displayLocationInfo() {
    // 定義每個景點的經緯度座標
    var point1Coords = point1;
    var point2Coords = point2;
    var point3Coords = point3;

    // 使用 Geocoder 將座標轉換為地點資訊並顯示
    var geocoder = new google.maps.Geocoder();
    geocoder.geocode({ location: point1Coords }, function (results, status) {
        if (status === 'OK') {
            var point1Location = results[0].formatted_address; // 取得第一個景點地點資訊
            document.getElementById('point1').textContent = '第一個景點: ' + point1Location; // 顯示在 cshtml 中的第一個景點 <p> 標籤中
        }
    });
    geocoder.geocode({ location: point2Coords }, function (results, status) {
        if (status === 'OK') {
            var point2Location = results[0].formatted_address; // 取得第二個景點地點資訊
            document.getElementById('point2').textContent = '第二個景點: ' + point2Location; // 顯示在 cshtml 中的第二個景點 <p> 標籤中
        }
    });
    geocoder.geocode({ location: point3Coords }, function (results, status) {
        if (status === 'OK') {
            var point3Location = results[0].formatted_address; // 取得第三個景點地點資訊
            document.getElementById('point3').textContent = '第三個景點: ' + point3Location; // 顯示在 cshtml 中的第三個景點 <p> 標籤中
        }
    });
}
//=============================================MAP End============================================================

//=============================================標籤按鈕start=======================================================
var vueApp = {
    data() {
        return {
            tags: [],//資料庫Tags
            selectedtags: [],//選取的按鈕
            photoUrl: null,
            hotelArray: [],
            apiKey: "AIzaSyCU-3dwpUCeWt2kS73Ytk4-U5aG2wasIxo"
        }
    },
    methods: {
        searchNearby(lodginglat, lodginglng) {
            let _this = this;
            var request = {
                location: `${lodginglat},${lodginglng}`,
                key: this.apiKey
            };
            const url = "https://localhost:7238/Home/GetLodging";

            const queryString = new URLSearchParams(request).toString();
            const fullUrl = `${url}?${queryString}`;
            console.log(fullUrl);
            axios.get(fullUrl).then(response => {
                console.log(JSON.parse(response.data).results);
                Data = JSON.parse(response.data);
                _this.hotelArray = Data.results;

            }).catch(err => {
                console.log(err);
            })
        },
        getPhotoUrl(photoUrl) {
            return `https://maps.googleapis.com/maps/api/place/photo?maxwidth=400&photoreference=${photoUrl}&key=${this.apiKey}`;
        },
        getTags() {
            var url = "https://localhost:7238/Home/GetTags";
            axios.get(url).then(response => {
                this.tags = response.data;
            }).catch(err => {
                console.log(err);
            })
        },
        addTags() {
            //把點選按鈕的value放入selectedtags
            var tagValue = event.target.value;
            this.selectedtags.push(tagValue);
            //被點擊的按鈕設定
            const clickedButton = event.target;
            clickedButton.disabled = true;
            clickedButton.style.backgroundColor = "#91b9ff";
            this.renderTags();
        },
        renderTags() {
            let _this = this;
            var tagsContainer = _this.$refs.tagsContainer;
            tagsContainer.innerHTML = '';
            //生成被點選的按鈕,放進ref="tagsContainer"
            _this.selectedtags.forEach(function (tag) {
                var tagElement = document.createElement('div');
                tagElement.classList.add('tag');
                tagElement.textContent = tag;
                //添加刪除按鈕
                var deleteButton = document.createElement('button');
                deleteButton.textContent = 'x';
                deleteButton.classList.add('delete-button');
                deleteButton.addEventListener('click', () => {
                    var index = _this.selectedtags.indexOf(tag);
                    if (index != -1) {
                        _this.selectedtags.splice(index, 1);
                        // 找到對應的.btn1按鈕
                        var correspondingButton = document.querySelector('.btn1[value="' + tag + '"]');
                        correspondingButton.disabled = false;
                        correspondingButton.style.backgroundColor = "transparent";
                        tagsContainer.removeChild(tagElement);
                    }
                });
                // 將刪除按鈕添加到tag元素中
                tagElement.appendChild(deleteButton);
                // 將tag元素添加到tags容器中
                tagsContainer.appendChild(tagElement);
            });
        },
    },
    mounted: function () {
        this.getTags();
        this.searchNearby(22.669716027449518, 120.30233334390414);
    }
};

//=============================================標籤按鈕end=======================================================
