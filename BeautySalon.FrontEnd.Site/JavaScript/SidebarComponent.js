const SidebarComponent = {
    name: "SidebarComponent",
    template: `
    <aside class="sidebar">
      <nav class="sidebar-nav">
        <a
          v-for="(item, index) in sidebarItems"
          :key="index"
          href="#"
          :class="['sidebar-link', { active: index === activeIndex }]"
          @click.prevent="handleNavigation(item, index)"
        >
          {{ item.text }}
        </a>
      </nav>
    </aside>
  `,
    setup() {
        const { ref, onMounted } = Vue;
        const { isAuthenticated } = useAuth();

        const sidebarItems = [
            { text: "會員資料編輯", link: "/Html/MemberInfo.html", requiresAuth: true },
            { text: "我的訂單", link: "/Html/Orderlist.html", requiresAuth: true },
            { text: "我的預約", link: "/Html/AppointmentDetail.html", requiresAuth: true },
        ];

        const activeIndex = ref(0);

        const navigateTo = (path) => {
            window.location.href = path;
        };

        const handleNavigation = (item, index) => {
            if (item.requiresAuth && !isAuthenticated.value) {
                // 如果需要驗證且未登入，跳轉到登入頁面
                navigateTo("/Login.html");
            } else {
                activeIndex.value = index;
                navigateTo(item.link);
            }
        };

        const updateActiveIndex = () => {
            const currentPath = window.location.pathname;
            const foundIndex = sidebarItems.findIndex(
                (item) => item.link === currentPath
            );
            activeIndex.value = foundIndex !== -1 ? foundIndex : 0;
        };

        onMounted(() => {
            updateActiveIndex();
        });

        return {
            sidebarItems,
            activeIndex,
            handleNavigation,
        };
    },
};
