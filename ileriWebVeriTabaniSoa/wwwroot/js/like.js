$(document).on('click', '.like-button', function () {
    var button = $(this);
    var postId = button.data('post-id'); // Butona bağlı postId'yi al

    $.ajax({
        url: '/Posts/LikePost', // Beğeni ekleme işlemi yapılacak URL
        type: 'POST',
        data: { postId: postId },
        success: function (response) {
            // Beğeni sayısını güncelle
            button.find('.like-count').text(response.likeCount);

            // Beğeni butonunun durumunu güncelle (aktif/pasif)
            if (response.liked) {
                button.addClass('liked');  // Butonu 'liked' sınıfı ile işaretle
            } else {
                button.removeClass('liked');  // 'liked' sınıfını kaldır
            }
        },
        error: function () {
            alert('Bir hata oluştu.');
        }
    });
});
