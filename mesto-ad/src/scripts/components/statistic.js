const createStatisticElement = (title, value) => {
    const infoElem = document.getElementById("popup-info-definition-template").content.querySelector(".popup__info-item").cloneNode(true);

    infoElem.querySelector(".popup__info-term").textContent = title;
    infoElem.querySelector(".popup__info-description").textContent = value;
    return infoElem;
};

const createListElement = (name) => {
    const listElem = document.getElementById("popup-info-user-preview-template").content.querySelector(".popup__list-item").cloneNode(true);

    listElem.textContent = name;
    return listElem;
};

export const clearStatisticInfo = (logoInfoTitle, logoInfoSecondTitle, logoInfoDescription, logoInfoList) => {
    logoInfoTitle.textContent = "";
    logoInfoSecondTitle.textContent = "";
    logoInfoDescription.innerHTML = "";
    logoInfoList.textContent = "";
};

export const getUserCount = (cards) => {
    let userCount = new Set();
    cards.forEach((card) => {
      const ownerId = card.owner._id;

      userCount.add(ownerId);
    });
    return userCount;
};

export const getUserStats = (cards) => {
    let userStats = new Map();
    cards.forEach((card) => {
        if(card.likes.length != 0) {
            card.likes.forEach((like) => {
            const ownerLikeId = like._id;
            const ownerLikeName = like.name;
        
            if(userStats.has(ownerLikeId)) {
                userStats.get(ownerLikeId).likeCount++;
            }
            else {
                userStats.set(ownerLikeId, { name: ownerLikeName, likeCount: 1 });
            } 
        
            });
        };
    });
    return userStats;
};

export const getPopularCards = (cards) => {
    let popularCards = [];
    cards.forEach((card) => {
        popularCards.push({ name: card.name, likeCount: card.likes.length });
    });
    return popularCards;
};

export const setStats = (logoInfoDescription, userCount, userStatsSort, likesCount) => {
    const info = [
        {
            title: "Всего пользователей:",
            value: userCount.size
        },
          {
            title: "Всего лайков:",
            value: likesCount
          },
          {
            title: "Максимальное количество лайков от одного:",
            value: userStatsSort.entries().next().value[1].likeCount
          },
          {
            title: "Чемпион лайков:",
            value: userStatsSort.entries().next().value[1].name
          }
    ];
    
    info.forEach(({ title, value }) => {
      logoInfoDescription.append(createStatisticElement(title, value));
    });
};

export const setPopularCards = (popularCards, logoInfoList) => {
    popularCards.slice(0, 3).forEach(({ name, likeCount }) => {
      logoInfoList.append(createListElement(name));
    });
};