# FROM debian

# RUN apt-get update && apt-get upgrade -y
# # mysql-server
# RUN apt install -y pip
# RUN pip install flask
FROM python
RUN pip install flask flask_restful mysql-connector-python
ADD ArtichokePlant/ArtichokePlant.py ./
CMD ["python", "ArtichokePlant.py"]
EXPOSE 5000