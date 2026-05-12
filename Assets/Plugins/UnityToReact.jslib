mergeInto(LibraryManager.library, {
  NextButton: function() {
    if (typeof window !== 'undefined' && window.NextButton) {
      try {
        console.log("jslib: calling window.NextButton()");
        window.NextButton();
      } catch (e) {
        console.log("jslib: window.NextButton threw:", e);
      }
    } else {
      console.log("jslib: window.NextButton not found");
    }
  },
  BackButton: function() {
    if (typeof window !== 'undefined' && window.BackButton) {
      try {
        console.log("jslib: calling window.BackButton()");
        window.BackButton();
      } catch (e) {
        console.log("jslib: window.BackButton threw:", e);
      }
    } else {
      console.log("jslib: window.BackButton not found");
    }
  }
});