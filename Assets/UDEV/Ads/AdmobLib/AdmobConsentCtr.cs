#if USE_ADMOB
using GoogleMobileAds.Ump.Api;
#endif
using System;
using UDEV.DMTool;
using UnityEngine;
using UnityEngine.UI;

namespace UDEV
{
    /// <summary>
    /// Helper class that implements consent using the Google User Messaging Platform (UMP) SDK.
    /// </summary>
    public class AdmobConsentCtr : MonoBehaviour
    {
#if USE_ADMOB
        /// <summary>
        /// If true, it is safe to call MobileAds.Initialize() and load Ads.
        /// </summary>
        public bool CanRequestAds => ConsentInformation.CanRequestAds();

        [SerializeField, Tooltip("Button to show user consent and privacy settings.")]
        private Button _privacyButton;

        private void Start()
        {
            // Disable the privacy settings button.
            if (_privacyButton != null)
            {
                _privacyButton.interactable = false;
            }
        }

        /// <summary>
        /// Startup method for the Google User Messaging Platform (UMP) SDK
        /// which will run all startup logic including loading any required
        /// updates and displaying any required forms.
        /// </summary>
        public void GatherConsent(Action<string> onComplete)
        {
            Debug.Log("Gathering consent.");

            var requestParameters = new ConsentRequestParameters
            {
                // False means users are not under age.
                TagForUnderAgeOfConsent = false,
                ConsentDebugSettings = new ConsentDebugSettings
                {
                    // For debugging consent settings by geography.
                    DebugGeography = DebugGeography.Disabled,
                    // https://developers.google.com/admob/unity/test-ads
                    TestDeviceHashedIds = AdmobController.TestDeviceIds,
                }
            };

            // Combine the callback with an error popup handler.
            onComplete = (onComplete == null)
                ? UpdateErrorPopup
                : onComplete + UpdateErrorPopup;

            // The Google Mobile Ads SDK provides the User Messaging Platform (Google's
            // IAB Certified consent management platform) as one solution to capture
            // consent for users in GDPR impacted countries. This is an example and
            // you can choose another consent management platform to capture consent.
            ConsentInformation.Update(requestParameters, (FormError updateError) =>
            {
                // Enable the change privacy settings button.
                UpdatePrivacyButton();

                if (updateError != null)
                {
                    onComplete(updateError.Message);
                    return;
                }

                // Determine the consent-related action to take based on the ConsentStatus.
                if (CanRequestAds)
                {
                    // Consent has already been gathered or not required.
                    // Return control back to the user.
                    onComplete(null);
                    return;
                }

                // Consent not obtained and is required.
                // Load the initial consent request form for the user.
                ConsentForm.LoadAndShowConsentFormIfRequired((FormError showError) =>
                {
                    UpdatePrivacyButton();
                    if (showError != null)
                    {
                        // Form showing failed.
                        if (onComplete != null)
                        {
                            onComplete(showError.Message);
                        }
                    }
                    // Form showing succeeded.
                    else if (onComplete != null)
                    {
                        onComplete(null);
                    }
                });
            });
        }

        /// <summary>
        /// Shows the privacy options form to the user.
        /// </summary>
        /// <remarks>
        /// Your app needs to allow the user to change their consent status at any time.
        /// Load another form and store it to allow the user to change their consent status
        /// </remarks>
        public void ShowPrivacyOptionsForm(Action<string> onComplete)
        {
            Debug.Log("Showing privacy options form.");

            // combine the callback with an error popup handler.
            onComplete = (onComplete == null)
                ? UpdateErrorPopup
                : onComplete + UpdateErrorPopup;

            ConsentForm.ShowPrivacyOptionsForm((FormError showError) =>
            {
                UpdatePrivacyButton();
                if (showError != null)
                {
                    // Form showing failed.
                    if (onComplete != null)
                    {
                        onComplete(showError.Message);
                    }
                }
                // Form showing succeeded.
                else if (onComplete != null)
                {
                    onComplete(null);
                }
            });
        }

        /// <summary>
        /// Reset ConsentInformation for the user.
        /// </summary>
        public void ResetConsentInformation()
        {
            ConsentInformation.Reset();
            UpdatePrivacyButton();
        }

        void UpdatePrivacyButton()
        {
            if (_privacyButton != null)
            {
                _privacyButton.interactable =
                    ConsentInformation.PrivacyOptionsRequirementStatus ==
                        PrivacyOptionsRequirementStatus.Required;
            }
        }

        void UpdateErrorPopup(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            YesNoDialog messageDialog = (YesNoDialog) DialogDB.Ins.GetDialog(DialogType.Message);
            if (messageDialog != null)
            {
                messageDialog.message.SetText(message);
                DialogDB.Ins.Show(messageDialog);
            }
        }

#endif
    }
}