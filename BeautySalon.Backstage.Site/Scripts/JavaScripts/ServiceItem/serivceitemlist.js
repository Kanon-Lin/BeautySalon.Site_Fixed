
    const {createApp, ref, onMounted} = Vue;

    const config = {
        setup() {
        const searchTerm = ref('');
    const services = ref([]);
    const filteredServices = ref([]);

        // 從 API 獲取資料 (使用 fetch)
        const fetchServices = () => {
            fetch('https://localhost:44302/api/ProductsApi/ProductIndext')
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json(); // 將回應轉換為 JSON
            })
                .then(data => {
                console.log('Fetched data:', data); 
                services.value = data;
                filteredServices.value = services.value;  // 初始時顯示所有服務
            })
            .catch(error => {
                console.error('Error fetching services:', error);
            });
        };

        // 搜尋功能
            const updateFilteredService = (servicesData) => {
                if (!Array.isArray(servicesData)) {
                    console.error('獲取的數據不是數組:', servicesData);
                    return; // 停止執行，如果不是數組
                }

                // 當 searchTerm 為空時，顯示所有服務項目
                if (searchTerm.value.trim() === '') {
                    filteredServices.value = servicesData; // 將所有獲取到的服務賦值給 filteredServices
                } else {
                    // 否則，過濾出匹配的服務項目
                    filteredServices.value = servicesData.filter(s =>
                        (s.ProductName && s.ProductName.includes(searchTerm.value)) ||
                        (s.Description && s.Description.includes(searchTerm.value))
                    );
                }
            };

            const searchService = () => {
                let url;

                // 檢查輸入值是否為空
                if (searchTerm.value.trim() === '') {
                    url = `https://localhost:44302/api/ProductsApi/ProductIndext`; // 獲取所有服務的 API
                } else {
                    url = `https://localhost:44302/api/ProductsApi/SearchProduct?searchTerm=${encodeURIComponent(searchTerm.value)}`;
                }

                fetch(url)
                    .then(response => {
                        if (!response.ok) {
                            throw new Error('Network response was not ok');
                        }
                        return response.json();
                    })
                    .then(data => {
                        console.log('獲取的服務數據:', data); // 檢查返回的數據
                        updateFilteredService(data);
                    })
                    .catch(error => {
                        console.error('無法獲取服務:', error);
                    });
            };





        // 新增、編輯、刪除服務
        const addService = () => {
        window.location.href = `新增服務項目.html?mode=create`;
        };

            const editService = (ProductId) => {
            window.location.href = `編輯服務項目.html?id=${ProductId}&mode=edit`;
        };

            const deleteService = (ProductId) => {
            window.location.href = `刪除服務項目.html?id=${ProductId}&mode=delete`;
        };

        // 在組件掛載時自動呼叫 API 來獲取數據
        onMounted(() => {
        fetchServices();
        });

    return {
        searchTerm,
        services,
        filteredServices,
        searchService,
        addService,
        editService,
        deleteService
    };
    }
};

    const app = createApp(config);
    app.mount('#app');

