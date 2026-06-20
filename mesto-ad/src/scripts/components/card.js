export const toogleLikeCard = (likeButton, cardLikeCount, likesCount) => {
  likeButton.classList.toggle("card__like-button_is-active");
  cardLikeCount.textContent = likesCount;
};

export const removeCard = (cardElement) => {
  cardElement.remove();
};

const getTemplate = () => {
  return document
    .getElementById("card-template")
    .content.querySelector(".card")
    .cloneNode(true);
};

export const createCardElement = (
  data,
  { onPreviewPicture, 
    onLikeIcon, 
    onDeleteCard,
    currentUserId,
    isLiked
  }
) => {
  const cardElement = getTemplate();
  const likeButton = cardElement.querySelector(".card__like-button");
  const deleteButton = cardElement.querySelector(".card__control-button_type_delete");
  const cardImage = cardElement.querySelector(".card__image");
  const cardLikeCount = cardElement.querySelector(".card__like-count");

  const likesCount = data.likes.length;

  cardImage.src = data.link;
  cardImage.alt = data.name;
  cardElement.querySelector(".card__title").textContent = data.name;
  cardLikeCount.textContent = likesCount;

  if(isLiked) {
    likeButton.classList.add("card__like-button_is-active");
  }

  if (onLikeIcon) {
    likeButton.addEventListener("click", () => {
      const isLiked = likeButton.classList.contains("card__like-button_is-active");
      onLikeIcon(data._id, isLiked, likeButton, cardLikeCount)
    });
  }

  if(data.owner && data.owner._id === currentUserId) {
    if (onDeleteCard) {
      deleteButton.addEventListener("click", () => onDeleteCard(data._id, cardElement));
    }
  }
  else {
    deleteButton.remove();
  }

  if (onPreviewPicture) {
    cardImage.addEventListener("click", () => onPreviewPicture({name: data.name, link: data.link}));
  }

  return cardElement;
};
