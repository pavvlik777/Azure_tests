async function getTimezonesAsync() {
    const response = await fetch('/api/time');
    const data = response.json();

    return data;
}

async function getDiffAsync(timezoneId) {
    const response = await fetch('/api/time/diff/minsk/' + timezoneId, {
        method: "post"
    });
    const data = response.json();

    return data;
}

async function updateTimeAsync() {
    const date = new Date();
    const timezones = await getTimezonesAsync();
    for (const timezone of timezones) {
        const timezoneDiv = document.getElementById("timezone_" + timezone.zoneId);
        const utc = date.getTime() + (date.getTimezoneOffset() * 60000);
        const current = new Date(utc + timezone.utcOffsetMinutes * 60000);
        const timezoneTime = timezoneDiv.querySelector(".timezone__time");
        timezoneTime.textContent = current.toLocaleString();
    }

    setTimeout(updateTimeAsync, 500);
}


(async () => {
    const timezones = await getTimezonesAsync();
    const timezoneContainer = document.getElementById("timezones");
    const timezoneTemplate = timezoneContainer.querySelector("#timezone-template");
    for (const timezone of timezones) {
        let newTimezone = timezoneTemplate.cloneNode(true);
        timezoneTemplate.after(newTimezone);

        newTimezone.style.display = '';

        const timezoneName = newTimezone.querySelector(".timezone__name");
        timezoneName.textContent = timezone.displayName;
        newTimezone.id = 'timezone_' + timezone.zoneId;

        const timezoneDiffButton = newTimezone.querySelector(".timezone__diff");
        timezoneDiffButton.onclick = async () => {
            const diffContaienr = document.getElementById("diff-container");
            const diff = await getDiffAsync(timezone.zoneId);
            diffContaienr.textContent = diff / 60 + ' hours';
        }
    }

    await updateTimeAsync();
})();