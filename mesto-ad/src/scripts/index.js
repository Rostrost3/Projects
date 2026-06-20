/*
  Файл index.js является точкой входа в наше приложение
  и только он должен содержать логику инициализации нашего приложения
  используя при этом импорты из других файлов

  Из index.js не допускается что то экспортировать
*/

import { createCardElement, removeCard, toogleLikeCard } from "./components/card.js";
import { openModalWindow, closeModalWindow, setCloseModalWindowEventListeners } from "./components/modal.js";
import { enableValidation, clearValidation } from "./components/validation.js";
import { getUserInfo, getCardList, setUserInfo, setUserAvatar, addNewCard, deleteCard, changeLikeCardStatus } from "./components/api.js";
import { clearStatisticInfo, getUserCount, getUserStats, getPopularCards, setStats, setPopularCards } from "./components/statistic.js";

// DOM узлы
const placesWrap = document.querySelector(".places__list");
const profileFormModalWindow = document.querySelector(".popup_type_edit");
const profileForm = profileFormModalWindow.querySelector(".popup__form");
const profileTitleInput = profileForm.querySelector(".popup__input_type_name");
const profileDescriptionInput = profileForm.querySelector(".popup__input_type_description");

const cardFormModalWindow = document.querySelector(".popup_type_new-card");
const cardForm = cardFormModalWindow.querySelector(".popup__form");
const cardNameInput = cardForm.querySelector(".popup__input_type_card-name");
const cardLinkInput = cardForm.querySelector(".popup__input_type_url");

const imageModalWindow = document.querySelector(".popup_type_image");
const imageElement = imageModalWindow.querySelector(".popup__image");
const imageCaption = imageModalWindow.querySelector(".popup__caption");

const openProfileFormButton = document.querySelector(".profile__edit-button");
const openCardFormButton = document.querySelector(".profile__add-button");

const profileTitle = document.querySelector(".profile__title");
const profileDescription = document.querySelector(".profile__description");
const profileAvatar = document.querySelector(".profile__image");

const avatarFormModalWindow = document.querySelector(".popup_type_edit-avatar");
const avatarForm = avatarFormModalWindow.querySelector(".popup__form");
const avatarInput = avatarForm.querySelector(".popup__input");

const logoModalWindow = document.querySelector(".popup_type_info");
const logoInfo = logoModalWindow.querySelector(".popup__content");
const logoInfoTitle = logoInfo.querySelector(".popup__title");
const logoInfoDescription = logoInfo.querySelector(".popup__info");
const logoInfoSecondTitle = logoInfo.querySelector(".popup__text");
const logoInfoList = logoInfo.querySelector(".popup__list");
const headerLogoImg = document.querySelector(".header__logo");


let currentUserId = null;

const handlePreviewPicture = ({ name, link }) => {
  imageElement.src = link;
  imageElement.alt = name;
  imageCaption.textContent = name;
  openModalWindow(imageModalWindow);
};

const handleProfileFormSubmit = (evt) => {
  evt.preventDefault();

  const formSubmitButton = profileForm.querySelector(".popup__button");
  const originalText = formSubmitButton.textContent;
  
  formSubmitButton.textContent = "Сохранение...";
  formSubmitButton.classList.add("popup__button_disabled");
  
  setUserInfo({
    name: profileTitleInput.value,
    about: profileDescriptionInput.value
  }).then((userData) => {
      profileTitle.textContent = userData.name;
      profileDescription.textContent = userData.about;
      closeModalWindow(profileFormModalWindow);
  })
  .catch((err) => { 
    console.log(err);
  })
  .finally(() => {
    formSubmitButton.textContent = originalText;
    formSubmitButton.classList.remove("popup__button_disabled");
  });
};

const handleAvatarFormSubmit = (evt) => {
  evt.preventDefault();

  const formSubmitButton = avatarForm.querySelector(".popup__button");
  const originalText = formSubmitButton.textContent;
  
  formSubmitButton.textContent = "Сохранение...";
  formSubmitButton.classList.add("popup__button_disabled");

  setUserAvatar({
    avatar: avatarInput.value
  }).then((userData) => {
    profileAvatar.style.backgroundImage = `url(${userData.avatar})`;
    closeModalWindow(avatarFormModalWindow);
  })
  .catch((err) => { 
    console.log(err);
  })
  .finally(() => {
    formSubmitButton.textContent = originalText;
    formSubmitButton.classList.remove("popup__button_disabled");
  });
};

const handleCardFormSubmit = (evt) => {
  evt.preventDefault();

  const formSubmitButton = cardForm.querySelector(".popup__button");
  const originalText = formSubmitButton.textContent;
  
  formSubmitButton.textContent = "Создание...";
  formSubmitButton.classList.add("popup__button_disabled");

  addNewCard({
    name: cardNameInput.value,
    link: cardLinkInput.value
  }).then((newCard) => {
    const isLiked = false;

    placesWrap.prepend(
      createCardElement(newCard,
        {
          onPreviewPicture: handlePreviewPicture,
          onLikeIcon: handleLikeClick,
          onDeleteCard: handleDeleteClick,
          currentUserId,
          isLiked
        }
      )
    )

    closeModalWindow(cardFormModalWindow);
  })
  .catch((err) => { 
    console.log(err);
  })
  .finally(() => {
    formSubmitButton.textContent = originalText;
    formSubmitButton.classList.remove("popup__button_disabled");
  });
};

const handleDeleteClick = (cardId, cardElement) => {
  deleteCard({ cardId })
  .then(() => {
    removeCard(cardElement);
  })
  .catch((err) => { 
    console.log(err);
  });
};

const handleLikeClick = (cardId, isLiked, likeButton, cardLikeCount) => {
  changeLikeCardStatus({ cardID: cardId, isLiked: isLiked })
  .then((res) => {
    toogleLikeCard(likeButton, cardLikeCount, res.likes.length);
  })
  .catch((err) => { 
    console.log(err);
  });
};

const handleLogoClick = () => {
  getCardList()
  .then((cards) => {
    logoInfoTitle.textContent = "Статистика карточек";
    logoInfoSecondTitle.textContent = "Популярные карточки";

    let userCount = getUserCount(cards);
    let userStats = getUserStats(cards);
    let popularCards = getPopularCards(cards);

    const likesCount = popularCards.reduce((sum, card) => sum + card.likeCount, 0);

    const userStatsSort = new Map(Array.from(userStats).sort((a,b) => b[1].likeCount - a[1].likeCount));
    popularCards.sort((a,b) => b.likeCount - a.likeCount);

    setStats(logoInfoDescription, userCount, userStatsSort, likesCount);

    setPopularCards(popularCards, logoInfoList);
  })
  .catch((err) => { 
    console.log(err);
  });
};

// EventListeners
profileForm.addEventListener("submit", handleProfileFormSubmit);
cardForm.addEventListener("submit", handleCardFormSubmit);
avatarForm.addEventListener("submit", handleAvatarFormSubmit);

openProfileFormButton.addEventListener("click", () => {
  profileTitleInput.value = profileTitle.textContent;
  profileDescriptionInput.value = profileDescription.textContent;
  clearValidation(profileForm, validationSettings);
  openModalWindow(profileFormModalWindow);
});

profileAvatar.addEventListener("click", () => {
  avatarForm.reset();
  clearValidation(avatarForm, validationSettings);
  openModalWindow(avatarFormModalWindow);
});

openCardFormButton.addEventListener("click", () => {
  cardForm.reset();
  clearValidation(cardForm, validationSettings);
  openModalWindow(cardFormModalWindow);
});

headerLogoImg.addEventListener("click", () => {
  clearStatisticInfo(logoInfoTitle, logoInfoSecondTitle, logoInfoDescription, logoInfoList);
  openModalWindow(logoModalWindow);
  handleLogoClick();
})

//настраиваем обработчики закрытия попапов
const allPopups = document.querySelectorAll(".popup");
allPopups.forEach((popup) => {
  setCloseModalWindowEventListeners(popup);
});

const validationSettings = {
  formSelector: ".popup__form",
  inputSelector: ".popup__input",
  submitButtonSelector: ".popup__button",
  inactiveButtonClass: "popup__button_disabled",
  inputErrorClass: "popup__input_type_error",
  errorClass: "popup__error_visible",
};

enableValidation(validationSettings);

Promise.all([getCardList(), getUserInfo()])
  .then(([cards, userData]) => {
    currentUserId = userData._id;

    profileTitle.textContent = userData.name;
    profileDescription.textContent = userData.about;
    profileAvatar.style.backgroundImage = `url(${userData.avatar})`;

    cards.forEach((data) => {
        const isLiked = data.likes.some(like => like._id == currentUserId);

        placesWrap.append(
        createCardElement(data, {
          onPreviewPicture: handlePreviewPicture,
          onLikeIcon: handleLikeClick,
          onDeleteCard: handleDeleteClick,
          currentUserId,
          isLiked
        }),
      );
    });
  })
  .catch((err) => {
    console.log(err); // В случае возникновения ошибки выводим её в консоль
  });
