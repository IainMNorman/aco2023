(function() {
})(window.ismppMicrositeCore = window.ismppMicrositeCore || {});

(function(ismppMicrositeCore) {
  var formEl;
  var interactivePosterContainerEl;
  var formSuccessMessageEl;
  var formErrorMessageEl;
  var postUrl;
  var screenWidth;
  var isMobileDevice;

  ismppMicrositeCore.form = {
    init: function() {
      formEl = document.getElementById('interactive-poster-page-form');
      interactivePosterContainerEl = document.querySelector('.container--interactive-poster');
      formSuccessMessageEl = document.getElementById('form-success-message');
      formErrorMessageEl = document.getElementById('form-error-message');
      postUrl = '/interactive-poster';
      screenWidth = window.innerWidth;
      isMobileDevice = window.matchMedia('(any-pointer:coarse)').matches;

      if (formEl) {
        this.attachHandlers();
        this.isValid = new IsValid(formEl, {
          onFormValidated: function(e) {
            e.preventDefault();
            onFormValidated();
            return false;
          },
          onFormInvalidated: function(e) {
            onFormInvalidated();
            return false;
          }
        });
      }
    },
    attachHandlers: function() {
      window.addEventListener('resize', showFormForMobileDevices);
      showFormForMobileDevices();
    }
  };

  function onFormValidated() {
    var formBtnEl = formEl.querySelector('button[type="submit"]');
    formBtnEl.classList.add('is-disabled');

    post().then(function(response) {
      console.log(response.status);
      if (response.status === 200) {
        formEl.classList.add('is-hidden');
        formSuccessMessageEl.classList.remove('is-hidden');
      } else {
        formEl.reset();
        formBtnEl.classList.remove('is-disabled');
        formErrorMessageEl.classList.remove('is-hidden');
      }
    });
  }

  function onFormInvalidated() {
    var firstFormErrorEl = formEl.querySelector('.is-invalid');
    var offset = firstFormErrorEl.getBoundingClientRect().top + window.pageYOffset - 15;

    window.scroll({
      top: offset,
      behavior: 'smooth'
    });
  }

  function post() {
    var response = fetch(postUrl, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(serializeForm(formEl))
    });

    return response;
  }

  function serializeForm() {
    var data = serializeObject(formEl);

    return data;
  }

  function showFormForMobileDevices() {
    screenWidth = window.innerWidth;
    if (screenWidth < 768 && isMobileDevice) {
      formEl.classList.remove('is-hidden');
      interactivePosterContainerEl.classList.add('is-hidden');
    } else {
      formEl.classList.add('is-hidden');
      interactivePosterContainerEl.classList.remove('is-hidden');
    }
  }

  ismppMicrositeCore.form.init();
})(window.ismppMicrositeCore = window.ismppMicrositeCore || {});

(function(ismppMicrositeCore) {
  var isMobileDevice;
  var staticPosterContainerEl;
  var mobilePromptScreenEl;
  var screenHeight;
  var screenWidth;

  ismppMicrositeCore.mobilePromptScreen = {
    init: function() {
      isMobileDevice = window.matchMedia('(any-pointer:coarse)').matches;
      staticPosterContainerEl = document.querySelector('.container--static-poster');
      mobilePromptScreenEl = document.querySelector('.mobile-prompt-screen');
      mobilePromptHeaderEl = document.querySelector('.page-header--mobile-prompt-screen');
      screenHeight = window.innerHeight;
      screenWidth = window.innerWidth;

      if (mobilePromptScreenEl) {
        this.attachHandlers();
      }
    },
    attachHandlers: function() {
      window.addEventListener('resize', showMobilePromptScreen);
      showMobilePromptScreen();
    }
  };

  function showMobilePromptScreen() {
    screenHeight = window.innerHeight;
    screenWidth = window.innerWidth;

    if (screenHeight > screenWidth && isMobileDevice) {
      mobilePromptScreenEl.classList.remove('is-hidden');
      staticPosterContainerEl.classList.add('is-hidden');
    } else {
      mobilePromptScreenEl.classList.add('is-hidden');
      staticPosterContainerEl.classList.remove('is-hidden');
    }
  }

  ismppMicrositeCore.mobilePromptScreen.init();
})(window.ismppMicrositeCore = window.ismppMicrositeCore || {});

(function(ismppMicrositeCore) {
  var staticPosterContainer;
  var staticPoster;
  var zoomInButton;
  var zoomOutButton;

  ismppMicrositeCore.staticPoster = {
    init: function() {
      staticPosterContainer = document.getElementById('static-poster-container');
      staticPoster = document.getElementById('static-poster');
      zoomInButton = document.getElementById('zoom-in-button');
      zoomOutButton = document.getElementById('zoom-out-button');

      if (staticPosterContainer && staticPoster) {
        this.attachHandlers();
      }
    },
    attachHandlers: function() {
      this.panzoom = Panzoom(staticPoster, {
        maxScale: 10,
        minScale: 0.2
      });
      staticPoster.addEventListener('wheel', this.panzoom.zoomWithWheel);
      zoomInButton.addEventListener('click', this.panzoom.zoomIn);
      zoomOutButton.addEventListener('click', this.panzoom.zoomOut);
    }
  };
  ismppMicrositeCore.staticPoster.init();
})(window.ismppMicrositeCore = window.ismppMicrositeCore || {});

(function(ismppMicrositeCore) {
  var videoPlayer = document.querySelector('#video-player');

  ismppMicrositeCore.video = {
    init: function() {
      if (videoPlayer) {
        this.player = new Plyr(videoPlayer, {
          controls: ['play-large', 'play', 'progress', 'current-time', 'mute', 'volume', 'fullscreen'],
          loadSprite: true,
          iconUrl: './img/plyr.svg'
        });
      }
    }
  };

  ismppMicrositeCore.video.init();
})(window.ismppMicrositeCore = window.ismppMicrositeCore || {});
