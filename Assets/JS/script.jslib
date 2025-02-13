mergeInto(LibraryManager.library, {
    IsRunningOnMobile: function() {
        return /Mobi|Android|iPhone|iPad|iPod/i.test(navigator.userAgent);
    }
});
