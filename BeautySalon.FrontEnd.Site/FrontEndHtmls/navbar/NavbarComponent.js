export default {
  template: `
  <header class="main-header" :class="{ 'navbar-hidden': isNavbarHidden }">
    <nav class="navbar" :class="{ 'expanded': isExpanded }">
      <a @click.prevent="goToHomePage" href="#" class="logo-link">
        <img src="../images/navbar/logo.png" alt="Company Logo" class="logo" />
      </a>
        <div class="nav-links">
          <a v-for="(item, index) in navItems"
             :key="index"
             @click="handleNavigation(item)"
             class="nav-item"
             :class="{ 'disabled': !isAuthenticated && item.requiresAuth }">
            {{ item.text }}
          </a>

          <!-- 會員專區下拉 -->
          <div class="nav-item dropdown"
               @mouseenter="showMemberDropdown = true"
               @mouseleave="showMemberDropdown = false">
            <a href="#" class="nav-item" @click="handleMemberAreaClick">
              會員專區
            </a>
            <div v-if="showMemberDropdown && isAuthenticated" class="dropdown-menu">
              <a @click="navigateTo('/Membership-info/membership.html')" class="dropdown-item">會員資料編輯</a>
              <a @click="navigateTo('/Membership-orderlist/orderlist.html')" class="dropdown-item">我的訂單</a>
              <a @click="navigateTo('/Membership-orderlist_detail/orderdetail.html')" class="dropdown-item">我的預約</a>
            </div>
          </div>
        </div>

        <div class="user-actions">
        <div class="hamburger-menu d-md-none" @click="toggleExpand">
          <span></span>
          <span></span>
          <span></span>
        </div>

        <!-- 登入登出鈕 -->
        <div v-if="isAuthenticated" class="user-info" @mouseenter="showLogout = true" @mouseleave="showLogout = false">
            <span v-if="!showLogout" class="user-greeting">您好，{{ user.name }}</span>
            <button v-else @click="logout" class="logout-btn">
                <img src="/image/navbar/logout-icon.png" alt="Logout" class="logout-icon" />
                <span>登出</span>
            </button>
        </div>
        <a v-else @click="loginClick" class="login-btn">登入</a>
      </div>
    </nav>
  </header>
  `,
    setup() {
        const { ref, onMounted, onUnmounted, watch } = Vue;
        const { user, isAuthenticated, logout: authLogout } = useAuth();

        console.log('Initial auth state:', { isAuthenticated: isAuthenticated.value, user: user.value });

        // 添加一个 watch 監視認證狀態變化
        watch(isAuthenticated, (newValue) => {
            console.log('Authentication state changed:', newValue);
        });

    const showLogout = ref(false);
    const showMemberDropdown = ref(false);
    const isNavbarHidden = ref(false);
    const isExpanded = ref(false);

    const navItems = [
      { text: "關於我們", link: "/about-us", requiresAuth: false },
      { text: "服務項目", link: "../Products/Products.html", requiresAuth: false },
      { text: "店家資訊", link: "/store-info", requiresAuth: false },
      { text: "最新消息", link: "/news", requiresAuth: false },
      { text: "聯絡我們", link: "/contact", requiresAuth: false },
    ];

    const goToHomePage = () => {
        window.location.href = "/Html/HomePage.html";
    };

    const navigateTo = (path) => {
      window.location.href = path;
    };

      const handleNavigation = (item) => {
          if (item.requiresAuth && !isAuthenticated.value) {
              loginClick();
          } else {
              navigateTo(item.link);
          }
      };

      const handleMemberAreaClick = (event) => {
          if (!isAuthenticated.value) {
              event.preventDefault();
              loginClick();
          }
      };

      const logout = () => {
          authLogout();
          goToHomePage();
      };


    const loginClick = () => {
      navigateTo("/Login/Login.html");
    };

    const toggleExpand = () => {
      isExpanded.value = !isExpanded.value;
    };

    let lastScrollPosition = 0;

    const handleScroll = () => {
      const currentScrollPosition = window.pageYOffset;
      if (currentScrollPosition < 0) {
        return;
      }
      // 向下滾動時隱藏navbar，向上滾動時顯示navbar
      isNavbarHidden.value =
        currentScrollPosition > lastScrollPosition &&
        currentScrollPosition > 50;
      lastScrollPosition = currentScrollPosition;

      // 如果 navbar 是展開狀態，則在滾動時收起
      if (isExpanded.value) {
        isExpanded.value = false;
      }
    };

      onMounted(() => {
          window.addEventListener("scroll", handleScroll);
      });

      onUnmounted(() => {
          window.removeEventListener("scroll", handleScroll);
      });

    return {
        navItems,
        isAuthenticated,
        user,
        showLogout,
        showMemberDropdown,
        isNavbarHidden,
        logout,
        loginClick,
        handleNavigation,
        handleMemberAreaClick,
        goToHomePage,
        navigateTo,
        isExpanded,
        toggleExpand,
    };
  },
};
