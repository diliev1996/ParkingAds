import pika
url = 'amqp://sgxsvkdw:zmeCytKeq8wlXGgkn44Z23XU0cC1_aY1@bee.rmq.cloudamqp.com/sgxsvkdw'
params = pika.URLParameters(url)
connection = pika.BlockingConnection(params)

channel = connection.channel()

channel.exchange_declare(exchange='X', exchange_type='topic')
channel.queue_bind(exchange='X', queue='email-out', routing_key='#')

channel.basic_publish(exchange='X',
                      routing_key='o7808939@nwytg.net',
                      properties=pika.BasicProperties(
                          content_type='text/plain',
                          headers={'Subject': 'Greetings'}
                      ),
                      body='Email body......')

connection.close()
